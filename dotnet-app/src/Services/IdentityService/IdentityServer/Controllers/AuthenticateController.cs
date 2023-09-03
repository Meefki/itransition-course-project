using IdentityServer.Data;
using IdentityServer.ViewModels;
using IdentityServer4.Events;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using IdentityServer.ReturnUrlParsers;
using IdentityModel;

namespace IdentityServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController 
        : ControllerBase
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly AuthorizationDbContext dbContext;

        public AuthenticateController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            AuthorizationDbContext dbContext)
        {
            this.interaction = interaction;
            this.events = events;
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM vm)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Email == vm.Email && x.PasswordHash == vm.PasswordHash);

            if (user is not null)
            {
                if (user.Email == vm.Email)
                {
                    return BadRequest("User already exists");
                }

                return BadRequest("User with this login already exists");
            }

            user = new()
            {
                UserName = vm.Username,
                PasswordHash = vm.PasswordHash,
                Email = vm.Email,
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            LoginVM loginVM = new()
            {
                Email = vm.Email,
                PasswordHash = vm.PasswordHash,
                RememberMe = false,
            };
            return await Login(loginVM);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM vm)
        {
            string queryString = HttpContext.Request.QueryString.ToString();
            string returnUrlSearch = Uri.UnescapeDataString(queryString);
            string returnUrl = string.IsNullOrEmpty(returnUrlSearch) ? "" : returnUrlSearch["?ReturnUrl=".Length..];
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            var user = dbContext.Users.FirstOrDefault(x =>
                        x.Email == vm.Email &&
                        x.PasswordHash == vm.PasswordHash);

            if (user is not null && context is not null)
            {
                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                AuthenticationProperties? props = null;
                if (vm.RememberMe) props = new AuthenticationProperties { IsPersistent = true };
                var isuser = new IdentityServerUser(user.Id) { DisplayName = user.UserName };

                await HttpContext.SignInAsync(isuser, props);
                string redirectUrl = QueryParamParser.GetParam<string>(returnUrl, OidcConstants.AuthorizeRequest.RedirectUri) ?? "/";
                return new JsonResult(new { RedirectUrl = Uri.UnescapeDataString(returnUrl), IsOk = true });
            }

            await events.RaiseAsync(new UserLoginFailureEvent(user?.UserName ?? vm.Email, "invalid credentials", clientId: context?.Client.ClientId));
            return new JsonResult(new { RedirectUrl = string.Empty, IsOk = false });
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout(string? logoutId = "")
        {
            var context = await interaction.GetLogoutContextAsync(logoutId ?? "");

            if ((User?.Identity?.IsAuthenticated ?? false) && context is not null)
            {
                Response.Cookies.Delete(IdentityServerConstants.DefaultCheckSessionCookieName);
                Response.Cookies.Delete(".AspNetCore.Identity.Application");
                await HttpContext.SignOutAsync();
            }

            return Ok(new 
            {
                clientName = string.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context?.ClientName,
                postLogoutRedirectUri = context?.PostLogoutRedirectUri,
                signOutIFrameUrl = context?.SignOutIFrameUrl,
                logoutId
            });
        }
    }
}

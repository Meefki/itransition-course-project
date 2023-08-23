using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(
            IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel, string action)
        {
            throw new NotImplementedException();
        }
    }
}

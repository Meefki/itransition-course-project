using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;

namespace Reviewing.API.Authorization;

public class ReviewEditHandler : AuthorizationHandler<ReviewEditRequirenment>
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public ReviewEditHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReviewEditRequirenment requirement)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            AuthorizationFailureReason reason = new(this, "HttpContext is null");
            context.Fail(reason);
            return;
        }

        var httpContext = httpContextAccessor.HttpContext;
        if (!httpContext.Request.Body.CanSeek)
        {
            httpContext.Request.EnableBuffering();
        }
        httpContext.Request.Body.Position = 0;
        var json = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        httpContext.Request.Body.Position = 0;
        dynamic? request = JsonConvert.DeserializeObject<dynamic>(json);

        if (request is null)
        {
            AuthorizationFailureReason reason = new(this, "Request is null");
            context.Fail(reason);
            return;
        }

        string? authorUserId = request["AuthorUserId"];
        string? subClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        string? adminClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role && x.Value == "admin")?.Value;
        if (string.IsNullOrEmpty(authorUserId) || (authorUserId != subClaim && adminClaim is null))
        {
            AuthorizationFailureReason reason = new(this, "Wrong value of authorUserId");
            context.Fail(reason);
            return;
        }

        context.Succeed(requirement);
    }
}
using Azure;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Stores;
using Newtonsoft.Json;
using System.Text;

namespace IdentityServer.Middleware;

public class ChallengeMiddleware
{
    private readonly RequestDelegate next;
    private readonly IServiceScopeFactory serviceScopeFactory;

    public ChallengeMiddleware(
        RequestDelegate next,
        IServiceScopeFactory serviceScopeFactory)
    {
        this.next = next;
        this.serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        string location = context.Response.Headers.Location!;
        var jsonObj = new
        {
            location = location
        };
        context.Response.Headers.Remove("Location");
        string json = JsonConvert.SerializeObject(jsonObj);
        byte[] data = Encoding.ASCII.GetBytes(json);

        if (context.Response.StatusCode == 302)
        {
            context.Response.StatusCode = 200;
        }
        await context.Response.Body.WriteAsync(new(data));
    }
}
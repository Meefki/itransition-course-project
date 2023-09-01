using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Extentions;

public static class CustomApplicationExtentions
{
    public static async Task InitializeDatabase(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

        var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();
        if (!context.Clients.Any())
        {
            foreach (var client in Config.GetClients(app.Configuration))
            {
                await context.Clients.AddAsync(client.ToEntity());
            }
            await context.SaveChangesAsync();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.GetApiResources())
            {
                await context.IdentityResources.AddAsync(resource.ToEntity());
            }
            await context.SaveChangesAsync();
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var apiScope in Config.GetApiScopes())
            {
                await context.ApiScopes.AddAsync(apiScope.ToEntity());
            }
            await context.SaveChangesAsync();
        }

        await scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>().Database.MigrateAsync();
    }
}
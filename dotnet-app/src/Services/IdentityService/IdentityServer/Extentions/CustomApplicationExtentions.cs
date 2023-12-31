﻿using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

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

        var userContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        await userContext.Database.MigrateAsync();

        if (!userContext.Roles.Any())
        {
            await userContext.Roles.AddRangeAsync(Config.GetRoles());
        }

        if (!userContext.RoleClaims.Any())
        {
            await userContext.RoleClaims.AddRangeAsync(Config.GetRoleClaims());
        }
    }

    public static string FindHttpsUrl(string[] urls, string scheme)
    {
        string host = "";
        foreach (var url in urls)
        {
            host = RemoveSchemeFromString(url, scheme);
            if (!string.IsNullOrEmpty(host))
                break;
        }
        return host;
    }

    public static string RemoveSchemeFromString(string url, string scheme)
    {
        int startIndex = url.StartsWith(scheme + "://") ? (scheme + "://").Length : -1;
        if (startIndex <= 0) return "";

        string host = url[startIndex..];
        return host;
    }
}
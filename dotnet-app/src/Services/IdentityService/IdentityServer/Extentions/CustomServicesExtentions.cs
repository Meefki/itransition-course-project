using IdentityServer.Data;
using IdentityServer.ReturnUrlParsers;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityServer.Extentions;

public static class CustomServicesExtentions
{
    //public static IServiceCollection AddCustom(this IServiceCollection services) { return services; }

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins("https://localhost:3000", "https://localhost:3001");
                policy.AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Authorization") ?? "";

        services.AddDbContext<AuthorizationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }

    public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        var assemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
        var authenticationConnectionString = configuration.GetConnectionString("Authentication");

        services
            .AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "https://localhost:3001";
                options.UserInteraction.ErrorUrl = "https://localhost:3001/error";
                options.UserInteraction.LogoutUrl = "https://localhost:3001/logout";
            })
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(authenticationConnectionString,
                    sql => sql.MigrationsAssembly(assemblyName));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(authenticationConnectionString,
                    sql => sql.MigrationsAssembly(assemblyName));
            });

        services.AddTransient<IReturnUrlParser, CustomReturnUrlParser>();

        return services;
    }
}
﻿using IdentityServer.Data;
using IdentityServer.ReturnUrlParsers;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityServer.Extentions;

public static class CustomServicesExtentions
{
    //public static IServiceCollection AddCustom(this IServiceCollection services) { return services; }

    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        string[] allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins(allowedOrigins);
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
        PasswordOptions passwordOptions = configuration.GetSection("Identity:Password").Get<PasswordOptions>(); 
        services
            .AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password = passwordOptions;
            })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        var assemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
        var authenticationConnectionString = configuration.GetConnectionString("Authentication");
        UserInteractionOptions userInteractionOptions = configuration.GetSection("Identity:UserInteraction").Get<UserInteractionOptions>();
        services
            .AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = userInteractionOptions.LoginUrl;
                options.UserInteraction.ErrorUrl = userInteractionOptions.ErrorUrl;
                options.UserInteraction.LogoutUrl = userInteractionOptions.LogoutUrl;
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
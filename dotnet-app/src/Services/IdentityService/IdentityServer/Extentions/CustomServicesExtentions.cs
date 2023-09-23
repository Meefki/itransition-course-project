using IdentityServer.Data;
using IdentityServer.IdentityServer;
using IdentityServer.IdentityServer.ReturnUrlParsers;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        services.AddTransient<IProfileService, ProfileService>();

        PasswordOptions passwordOptions = configuration.GetSection("Identity:Password").Get<PasswordOptions>(); 
        services
            .AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password = passwordOptions;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.ConsentCookie.IsEssential = true;
            options.CheckConsentNeeded = context => false;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.Secure = CookieSecurePolicy.Always;
        });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Events.OnRedirectToLogout = (context) =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        //services.AddAuthentication()
            //.AddGoogle(options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.ClientId = configuration["Authentication:Google:ClientId"]!;
            //    options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            //});

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
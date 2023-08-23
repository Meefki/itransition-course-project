using IdentityService.Configurations;
using IdentityService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace IdentityService;

internal static class CustomServicesConfigureExtention
{
    public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services, IConfiguration config)
    {
        ClientsConfiguration clientConfig = config.GetSection(ClientsConfiguration.SectionName).Get<ClientsConfiguration>();

        services.AddIdentityServer(options =>
        {
            options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
        })
        .AddAspNetIdentity<IdentityUser>()
        .AddInMemoryApiResources(Configuration.ApiResources)
        .AddInMemoryIdentityResources(Configuration.IdentityResources)
        .AddInMemoryApiScopes(Configuration.ApiScopes)
        .AddInMemoryClients(Configuration.Clients(clientConfig))
        .AddDeveloperSigningCredential();

        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = config.GetConnectionString("SqLiteDefault") ?? "";

        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }

    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(config =>
        {
            config.Password.RequiredLength = 4;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddCustomCookie(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "Reviews.Identity.Cookie";
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration config)
    {
        GoogleConfiguration googleConfig =
            config
                .GetSection(GoogleConfiguration.SectionName)
                .Get<GoogleConfiguration>();

        services.AddAuthentication(/*CookieAuthenticationDefaults.AuthenticationScheme*/);

            //.AddGoogle(options =>
            //{
            //    options.ClientId = googleConfig.ClientId;
            //    options.ClientSecret = googleConfig.ClientSecret;
            //});

            //.AddFacebook()

            //.AddTwitter()

            //.AddMicrosoftAccount();

        return services;
    }
}
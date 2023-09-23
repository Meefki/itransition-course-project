using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Reviewing.API.Filters;

namespace Reviewing.API.Extentions;

public static class CustomServicesExtention
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Reviewving") ?? "";
        services.AddDbContext<ReviewingDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(ReviewingDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        string[] origins = configuration.GetSection("Cors:AllowedHosts").Get<string[]>() ?? Array.Empty<string>();
        CorsPolicy policy = new();
        policy.Methods.Add("*");
        policy.Headers.Add("*");
        policy.SupportsCredentials = true;
        foreach (string origin in origins)
        {
            policy.Origins.Add(origin);
        }
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy);
        });

        return services;
    }

    public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = configuration["Identity:Url"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                };
            });

        return services;
    }

    public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        var openApi = configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return services;
        }

        services.AddEndpointsApiExplorer();

        return services.AddSwaggerGen(options =>
        {
            var document = openApi.GetRequiredSection("Document");

            var version = document.GetValue<string>("Version") ?? "v1";

            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = document.GetValue<string>("Title"),
                Version = version,
                Description = document.GetValue<string>("Description")
            });

            var identitySection = configuration.GetSection("Identity");

            if (!identitySection.Exists())
            {
                return;
            }

            var identityUrlExternal = identitySection["ExternalUrl"] ?? identitySection.GetValue<string>("Url");
            var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
                        TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
                        Scopes = scopes,
                    }
                }
            });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });
    }
}
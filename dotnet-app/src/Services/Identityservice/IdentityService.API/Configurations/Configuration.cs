using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityService.Configurations;

internal static class Configuration
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("ReviewsWebApi", "Web API"),
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("ReviewsWebApi", "Web API", new []
            { 
                JwtClaimTypes.Name,
                JwtClaimTypes.Role
            })
            {
                Scopes = {"ReviewsWebApi"},
            }
        };

    public static IEnumerable<Client> Clients(ClientsConfiguration clientConfig) =>
        new List<Client>
        {
            new Client()
            {
                ClientId = "reviews-web-api",
                ClientName = "Reviews Web API",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris = clientConfig.RedirectUris,
                AllowedCorsOrigins = clientConfig.AllowedCorsOrigins,
                PostLogoutRedirectUris = clientConfig.PostLogoutRedirectUris,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ReviewsWebApi"
                },
                AllowAccessTokensViaBrowser = true,
            },
            new Client()
            {
                ClientId = "reviews-web-app",
                ClientName = "Reviews Web App",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris = clientConfig.RedirectUris,
                AllowedCorsOrigins = clientConfig.AllowedCorsOrigins,
                PostLogoutRedirectUris = clientConfig.PostLogoutRedirectUris,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ReviewsWebApp"
                },
                AllowAccessTokensViaBrowser = true,
            }
        };
}
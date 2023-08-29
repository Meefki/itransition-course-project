using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<Client> GetClients(IConfiguration? configuration = null) =>
        new Client[]
        {
            new Client()
            {
                Enabled = true,
                ClientId = "js",
                ProtocolType = "oidc",
                RequireClientSecret = true,
                ClientName = "Js Static Site",
                Description = null,
                ClientUri = null,
                LogoUri = null,
                RequireConsent = false,
                AllowRememberConsent = true,
                AlwaysIncludeUserClaimsInIdToken = false,
                RequirePkce = true,
                AllowPlainTextPkce = false,
                RequireRequestObject = false,
                AllowAccessTokensViaBrowser = false,
                FrontChannelLogoutUri = null,
                FrontChannelLogoutSessionRequired = true,
                BackChannelLogoutUri = null,
                BackChannelLogoutSessionRequired = true,
                AllowOfflineAccess = true,
                IdentityTokenLifetime = (int)TimeSpan.FromMinutes(5).TotalSeconds,
                AllowedIdentityTokenSigningAlgorithms = null,
                AccessTokenLifetime = (int)TimeSpan.FromMinutes(40).TotalSeconds,
                AuthorizationCodeLifetime = (int)TimeSpan.FromMinutes(5).TotalSeconds,
                ConsentLifetime = null,
                AbsoluteRefreshTokenLifetime = (int)TimeSpan.FromDays(30).TotalSeconds,
                SlidingRefreshTokenLifetime = (int)TimeSpan.FromDays(15).TotalSeconds,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AccessTokenType = AccessTokenType.Jwt,
                EnableLocalLogin = true,
                IncludeJwtId = true,
                AlwaysSendClientClaims = false,
                ClientClaimsPrefix = "client_",
                PairWiseSubjectSalt = null,
                UserCodeType = null,
                DeviceCodeLifetime = (int)TimeSpan.FromMinutes(5).TotalSeconds,

                ClientSecrets = new Secret[]
                { 
                    new Secret { Value = "secret".Sha256() },
                },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:3000/callback" },
                PostLogoutRedirectUris = { "https://localhost:3000/" },
                AllowedCorsOrigins = { "https://localhost:3000", "https://localhost:3001" },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "reviewing"
                }
            }
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new ApiScope[]
        {
            new ApiScope("reviewing")
        };

    public static IEnumerable<IdentityResource> GetApiResources() =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
}
using IdentityModel;
using IdentityServer.IdentityServer;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<Client> GetClients(IConfiguration configuration = null) =>
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
                ClientUri = configuration["Clients:js:ClientUri"]!,
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
                RedirectUris = configuration.GetValue<string[]>("Clients:js:RedirectUris"),
                PostLogoutRedirectUris = configuration.GetValue<string[]>("Clients:js:PostLogoutRedirectUris"),
                AllowedCorsOrigins = configuration.GetValue<string[]>("Clients:js:AllowedCorsOrigins"),
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "name",
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
            new IdentityResources.Email()
        };

    public static IEnumerable<IdentityRole> GetRoles() =>
        new IdentityRole[]
        {
            new IdentityRole { Id = "0793d400-ae95-4fca-82a1-f7292202b569", Name = "admin" },
            new IdentityRole { Id = "a2d1a0cf-eee7-4658-ad96-0b5a7aaa34a4", Name = "user" },
        };

    public static IEnumerable<IdentityRoleClaim<string>> GetRoleClaims() =>
        new IdentityRoleClaim<string>[]
        {
            new IdentityRoleClaim<string> { Id = 0, RoleId = "0793d400-ae95-4fca-82a1-f7292202b569", ClaimType = ClaimTypes.Role, ClaimValue = "admin" },
            new IdentityRoleClaim<string> { Id = 1, RoleId = "a2d1a0cf-eee7-4658-ad96-0b5a7aaa34a4", ClaimType = ClaimTypes.Role, ClaimValue = "user" },
        };
}
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.Diagnostics;
using IdentityServer4.Stores;
using IdentityModel;

namespace IdentityServer.IdentityServer.ReturnUrlParsers;

public class CustomReturnUrlParser
    : IReturnUrlParser
{
    public static class ProtocolRoutePaths
    {
        public const string Authorize = "connect/authorize";
        public const string AuthorizeCallback = Authorize + "/callback";
    }

    private readonly IAuthorizeRequestValidator _validator;
    private readonly IUserSession _userSession;
    private readonly IClientStore clientStore;
    private readonly IConfiguration config;
    private readonly ILogger _logger;

    public CustomReturnUrlParser(
        IAuthorizeRequestValidator validator,
        IUserSession userSession,
        IClientStore clientStore,
        IConfiguration config,
        ILogger<CustomReturnUrlParser> logger)
    {
        _validator = validator;
        _userSession = userSession;
        this.clientStore = clientStore;
        this.config = config;
        _logger = logger;
    }

    public async Task<AuthorizationRequest?> ParseAsync(string returnUrl)
    {
        if (IsValidReturnUrl(returnUrl))
        {
            var parameters = returnUrl.ReadQueryStringAsNameValueCollection();
            var user = await _userSession.GetUserAsync();
            var result = await _validator.ValidateAsync(parameters, user);
            if (!result.IsError)
            {
                _logger.LogTrace("AuthorizationRequest being returned");
                return result.ValidatedRequest.ToAuthorizationRequest();
            }
        }

        _logger.LogTrace("No AuthorizationRequest being returned");
        return null;
    }

    public bool IsValidReturnUrl(string returnUrl)
    {
        string url = QueryParamParser.GetParam<string>(returnUrl, OidcConstants.AuthorizeRequest.RedirectUri) ?? string.Empty;

        if (url.IsLocalUrl())
        {
            _logger.LogTrace("returnUrl is valid");
            return true;
        }

        string clientId = QueryParamParser.GetParam<string>(returnUrl, OidcConstants.AuthorizeRequest.ClientId) ?? string.Empty;
        var client = clientStore.FindEnabledClientByIdAsync(clientId).Result;
        if (client is not null && (client.RedirectUris.Contains(url) || client.RedirectUris.Contains(Uri.UnescapeDataString(url))))
        {
            _logger.LogTrace("returnUrl is valid");
            return true;
        }

        if (client is null)
        {
            string message = "returnUrl is not valid. Cannot find a client: {0}";
            _logger.LogTrace(message, clientId);
        }
        else
        {
            string message = "returnUrl is not valid. Client ({0}) doesn't contains specified redirectUrl ({1})";
            _logger.LogTrace(message, clientId, url);
        }

        return false;
    }
}

internal static class Extensions
{
    [DebuggerStepThrough]
    public static NameValueCollection ReadQueryStringAsNameValueCollection(this string url)
    {
        if (url != null)
        {
            var idx = url.IndexOf('?');
            if (idx >= 0)
            {
                url = url.Substring(idx + 1);
            }
            var query = QueryHelpers.ParseNullableQuery(url);
            if (query != null)
            {
                return query.AsNameValueCollection();
            }
        }

        return new NameValueCollection();
    }

    [DebuggerStepThrough]
    public static bool IsLocalUrl(this string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        // Allows "/" or "/foo" but not "//" or "/\".
        if (url[0] == '/')
        {
            // url is exactly "/"
            if (url.Length == 1)
            {
                return true;
            }

            // url doesn't start with "//" or "/\"
            if (url[1] != '/' && url[1] != '\\')
            {
                return true;
            }

            return false;
        }

        // Allows "~/" or "~/foo" but not "~//" or "~/\".
        if (url[0] == '~' && url.Length > 1 && url[1] == '/')
        {
            // url is exactly "~/"
            if (url.Length == 2)
            {
                return true;
            }

            // url doesn't start with "~//" or "~/\"
            if (url[2] != '/' && url[2] != '\\')
            {
                return true;
            }

            return false;
        }

        return false;
    }

    [DebuggerStepThrough]
    internal static AuthorizationRequest ToAuthorizationRequest(this ValidatedAuthorizeRequest request)
    {
        var authRequest = new AuthorizationRequest
        {
            Client = request.Client,
            RedirectUri = request.RedirectUri,
            DisplayMode = request.DisplayMode,
            UiLocales = request.UiLocales,
            IdP = request.GetIdP(),
            Tenant = request.GetTenant(),
            LoginHint = request.LoginHint,
            PromptModes = request.PromptModes,
            AcrValues = request.GetAcrValues(),
            ValidatedResources = request.ValidatedResources,
        };

        authRequest.Parameters.Add(request.Raw);

        return authRequest;
    }

    [DebuggerStepThrough]
    public static NameValueCollection AsNameValueCollection(this IDictionary<string, StringValues> collection)
    {
        var nv = new NameValueCollection();

        foreach (var field in collection)
        {
            nv.Add(field.Key, field.Value.First());
        }

        return nv;
    }
}
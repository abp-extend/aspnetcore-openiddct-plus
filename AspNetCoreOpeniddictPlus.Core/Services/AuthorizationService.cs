using System.Security.Claims;
using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using IAuthorizationService = AspNetCoreOpeniddictPlus.Core.Interfaces.IAuthorizationService;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class AuthorizationService<IEntity>(
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictAuthorizationManager authorizationManager,
    IOpenIddictScopeManager scopeManager,
    SignInManager<IEntity> signInManager,
    UserManager<IEntity> userManager ) : Controller, IAuthorizationService 

    where IEntity : class
{
    public virtual async Task<IActionResult> AuthorizeAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
        
        // Try to retrieve the user principal stored in the authentication cookie and redirect
        // the user agent to the login page (or to an external provider) in the following cases:
        //
        //  - If the user principal can't be extracted or the cookie is too old.
        //  - If prompt=login was specified by the client application.
        //  - If a max_age parameter was provided and the authentication cookie is not considered "fresh" enough.
        //
        // For scenarios where the default authentication handler configured in the ASP.NET Core
        // authentication options shouldn't be used, a specific scheme can be specified here.
        var result = await HttpContext.AuthenticateAsync();
        if (!result.Succeeded || request.HasPromptValue(OpenIddictConstants.PromptValues.Login) ||
            (request.MaxAge != null && result.Properties?.IssuedUtc != null &&
             DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value)))
        {
            // If the client application requested promptless authentication,
            // return an error indicating that the user is not logged in.
            if (request.HasPromptValue(OpenIddictConstants.PromptValues.None))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.LoginRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                    }!));
                
            }
            // To avoid endless login -> authorization redirects, the prompt=login flag
            // is removed from the authorization request payload before redirecting the user.
            var prompt = string.Join(" ", request.GetPromptValues().Remove(OpenIddictConstants.PromptValues.Login));
            var parameters = Request.HasFormContentType ?
                Request.Form.Where(parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList() :
                Request.Query.Where(parameter => parameter.Key != OpenIddictConstants.Parameters.Prompt).ToList();

            parameters.Add(KeyValuePair.Create(OpenIddictConstants.Parameters.Prompt, new StringValues(prompt)));
            
            // For scenarios where the default challenge handler configured in the ASP.NET Core
            // authentication options shouldn't be used, a specific scheme can be specified here.
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Request.PathBase + Request.Path + QueryString.Create(parameters)
            });
        }
        // Retrieve the profile of the logged in user.
        var user = await userManager.GetUserAsync(result.Principal) ??
                   throw new InvalidOperationException("The user details cannot be retrieved.");
        
        // Retrieve the application details from the database.
        var application = await applicationManager.FindByClientIdAsync(request.ClientId) ??
                          throw new InvalidOperationException("Details concerning the calling client application cannot be found.");
        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = new List<object>();

        await foreach (var authorization in authorizationManager.FindAsync(
                           subject: await userManager.GetUserIdAsync(user),
                           client: await applicationManager.GetIdAsync(application),
                           status: OpenIddictConstants.Statuses.Valid,
                           type: OpenIddictConstants.AuthorizationTypes.Permanent,
                           scopes: request.GetScopes()))
        {
            authorizations.Add(authorization);
        }
        switch (await applicationManager.GetConsentTypeAsync(application))
        {
            // If the consent is external (e.g when authorizations are granted by a sysadmin),
            // immediately return an error if no authorization can be found in the database.
            case OpenIddictConstants.ConsentTypes.External when authorizations.Count is 0:
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }!));

            // If the consent is implicit or if an authorization was found,
            // return an authorization response without displaying the consent form.
            case OpenIddictConstants.ConsentTypes.Implicit:
            case OpenIddictConstants.ConsentTypes.External when authorizations.Count is not 0:
            case OpenIddictConstants.ConsentTypes.Explicit when authorizations.Count is not 0 && !request.HasPromptValue(OpenIddictConstants.PromptValues.Consent):
                // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: OpenIddictConstants.Claims.Name,
                    roleType: OpenIddictConstants.Claims.Role);

                // Add the claims that will be persisted in the tokens.
                identity.SetClaim(OpenIddictConstants.Claims.Subject, await userManager.GetUserIdAsync(user))
                        .SetClaim(OpenIddictConstants.Claims.Email, await userManager.GetEmailAsync(user))
                        .SetClaim(OpenIddictConstants.Claims.Name, await userManager.GetUserNameAsync(user))
                        .SetClaim(OpenIddictConstants.Claims.PreferredUsername, await userManager.GetUserNameAsync(user))
                        .SetClaims(OpenIddictConstants.Claims.Role, [.. (await userManager.GetRolesAsync(user))]);

                // Note: in this sample, the granted scopes match the requested scope
                // but you may want to allow the user to uncheck specific scopes.
                // For that, simply restrict the list of scopes before calling SetScopes.
                identity.SetScopes(request.GetScopes());
                var scopes = new List<string>();
                await foreach (var scope in scopeManager.ListResourcesAsync(identity.GetScopes()))
                {
                    scopes.Add(scope);
                }
                identity.SetResources(scopes);

                // Automatically create a permanent authorization to avoid requiring explicit consent
                // for future authorization or token requests containing the same scopes.
                var authorization = authorizations.LastOrDefault();
                authorization ??= await authorizationManager.CreateAsync(
                    identity: identity,
                    subject : await userManager.GetUserIdAsync(user),
                    client  : await applicationManager.GetIdAsync(application) ?? throw new InvalidOperationException(),
                    type    : OpenIddictConstants.AuthorizationTypes.Permanent,
                    scopes  : identity.GetScopes());

                identity.SetAuthorizationId(await authorizationManager.GetIdAsync(authorization));
                identity.SetDestinations(IncludeDestinationInAccessToken.Get);

                return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // At this point, no authorization was found in the database and an error must be returned
            // if the client application specified prompt=none in the authorization request.
            case OpenIddictConstants.ConsentTypes.Explicit   when request.HasPromptValue(OpenIddictConstants.PromptValues.None):
            case OpenIddictConstants.ConsentTypes.Systematic when request.HasPromptValue(OpenIddictConstants.PromptValues.None):
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "Interactive user consent is required."
                    }!));

            // In every other case, render the consent form.
            default: return Ok(new AuthorizationServiceResult
            {
                ApplicationName = await applicationManager.GetLocalizedDisplayNameAsync(application),
                Scope = request.Scope
            });
        }
    }

    public virtual async Task<IActionResult> LogoutAsync()
    {
        // Ask ASP.NET Core Identity to delete the local and external cookies created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        await signInManager.SignOutAsync();

        // Returning a SignOutResult will ask OpenIddict to redirect the user agent
        // to the post_logout_redirect_uri specified by the client application or to
        // the RedirectUri specified in the authentication properties if none was set.
        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    }

    public virtual async Task<IActionResult> ExchangeAsync()
    {
       var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

       
       if (request.IsClientCredentialsGrantType())
       {
           return await HandleExchangeClientCredentialsGrantType(request);
       }
       
       
       if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
           throw new InvalidOperationException("The specified grant type is not supported.");
       
       // Retrieve the claims principal stored in the authorization code/refresh token.
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        // Retrieve the user profile corresponding to the authorization code/refresh token.
        var user = await userManager.FindByIdAsync(result.Principal.GetClaim(OpenIddictConstants.Claims.Subject));
        if (user is null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                }!));
        }

        // Ensure the user is still allowed to sign in.
        if (!await signInManager.CanSignInAsync(user))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                }!));
        }

        var identity = new ClaimsIdentity(result.Principal.Claims,
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        // Override the user claims present in the principal in case they
        // changed since the authorization code/refresh token was issued.
        identity.SetClaim(OpenIddictConstants.Claims.Subject, await userManager.GetUserIdAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Email, await userManager.GetEmailAsync(user))
            .SetClaim(OpenIddictConstants.Claims.Name, await userManager.GetUserNameAsync(user))
            .SetClaim(OpenIddictConstants.Claims.PreferredUsername, await userManager.GetUserNameAsync(user))
            .SetClaims(OpenIddictConstants.Claims.Role, [.. (await userManager.GetRolesAsync(user))]);

        identity.SetDestinations(IncludeDestinationInAccessToken.Get);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

    }

    public virtual async Task<IActionResult> AcceptAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Retrieve the profile of the logged in user.
        var user = await userManager.GetUserAsync(User) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        // Retrieve the application details from the database.
        var application = await applicationManager.FindByClientIdAsync(request.ClientId) ??
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        
        var authorizations = new List<object>();

        await foreach (var auth in authorizationManager.FindAsync(
                           subject: await userManager.GetUserIdAsync(user),
                           client: await applicationManager.GetIdAsync(application),
                           status: OpenIddictConstants.Statuses.Valid,
                           type: OpenIddictConstants.AuthorizationTypes.Permanent,
                           scopes: request.GetScopes()))
        {
            authorizations.Add(auth);
        }
     

        // Note: the same check is already made in the other action but is repeated
        // here to ensure a malicious user can't abuse this POST-only endpoint and
        // force it to return a valid response without the external authorization.
        if (authorizations.Count is 0 && await applicationManager.HasConsentTypeAsync(application, OpenIddictConstants.ConsentTypes.External))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The logged in user is not allowed to access this client application."
                }!));
        }

        // Create the claims-based identity that will be used by OpenIddict to generate tokens.
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        // Add the claims that will be persisted in the tokens.
        identity.SetClaim(OpenIddictConstants.Claims.Subject, await userManager.GetUserIdAsync(user))
                .SetClaim(OpenIddictConstants.Claims.Email, await userManager.GetEmailAsync(user))
                .SetClaim(OpenIddictConstants.Claims.Name, await userManager.GetUserNameAsync(user))
                .SetClaim(OpenIddictConstants.Claims.PreferredUsername, await userManager.GetUserNameAsync(user))
                .SetClaims(OpenIddictConstants.Claims.Role, [.. (await userManager.GetRolesAsync(user))]);

        // Note: in this sample, the granted scopes match the requested scope
        // but you may want to allow the user to uncheck specific scopes.
        // For that, simply restrict the list of scopes before calling SetScopes.
        identity.SetScopes(request.GetScopes());
        var scopes = new List<string>();
        await foreach (var scope in scopeManager.ListResourcesAsync(identity.GetScopes()))
        {
            scopes.Add(scope);
        }
        identity.SetResources(scopes);

        // Automatically create a permanent authorization to avoid requiring explicit consent
        // for future authorization or token requests containing the same scopes.
        var authorization = authorizations.LastOrDefault();
        authorization ??= await authorizationManager.CreateAsync(
            identity: identity,
            subject : await userManager.GetUserIdAsync(user),
            client  : await applicationManager.GetIdAsync(application) ?? string.Empty,
            type    : OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes  : identity.GetScopes());

        identity.SetAuthorizationId(await authorizationManager.GetIdAsync(authorization));
        identity.SetDestinations(IncludeDestinationInAccessToken.Get);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    public virtual IActionResult Deny()
    {
        return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
    
    private async Task<IActionResult> HandleExchangeClientCredentialsGrantType(OpenIddictRequest request)
    {
        var application = await applicationManager.FindByClientIdAsync(request.ClientId);
        if (application == null)
        {
            throw new InvalidOperationException("The application details cannot be found in the database.");
        }

        // Create the claims-based identity that will be used by OpenIddict to generate tokens.
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        // Add the claims that will be persisted in the tokens (use the client_id as the subject identifier).
        var clientId = await applicationManager.GetClientIdAsync(application) ?? throw new InvalidOperationException("Client Id not found!");
        identity.AddClaim(OpenIddictConstants.Claims.Subject, clientId);
        identity.AddClaim(OpenIddictConstants.Claims.Name, await applicationManager.GetDisplayNameAsync(application) ?? "");

        // Note: In the original OAuth 2.0 specification, the client credentials grant
        // doesn't return an identity token, which is an OpenID Connect concept.
        //
        // As a non-standardized extension, OpenIddict allows returning an id_token
        // to convey information about the client application when the "openid" scope
        // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
        // scope is not explicitly set, no identity token is returned to the client application.

        // Set the list of scopes granted to the client application in access_token.
        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(request.GetScopes());
        var scopes = new List<string>();
        await foreach (var scope in scopeManager.ListResourcesAsync(identity.GetScopes()))
        {
            scopes.Add(scope);
        }
        principal.SetResources(scopes);

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(IncludeDestinationInAccessToken.Get(claim));
        }
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Helpers;
using AspNetCoreOpeniddictPlus.Core.Services;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using NuGet.Protocol;
using OpenIddict.Abstractions;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

public class AuthorizationController(
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictAuthorizationManager authorizationManager,
    IOpenIddictScopeManager scopeManager,
    SignInManager<OpeniddictPlusUser> signInManager,
    UserManager<OpeniddictPlusUser> userManager,
    ILogger<AuthorizationController> logger
)
    : AuthorizationService<OpeniddictPlusUser>(
        applicationManager,
        authorizationManager,
        scopeManager,
        signInManager,
        userManager
    )
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public override Task<IActionResult> AuthorizeAsync()
    {
        logger.LogInformation("AuthorizeAsync invoked");
        return base.AuthorizeAsync();
    }

    [Authorize, FormValueRequired("submit.Deny")]
    [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
    public override IActionResult Deny()
    {
        logger.LogWarning("Deny request invoked");
        return base.Deny();
    }

    [Authorize, FormValueRequired("submit.Accept")]
    [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
    public override Task<IActionResult> AcceptAsync()
    {
        logger.LogInformation("Accept request invoked");
        return base.AcceptAsync();
    }

    [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
    public override Task<IActionResult> ExchangeAsync()
    {
        logger.LogInformation("ExchangeAsync request invoked");
        return base.ExchangeAsync();
    }
}

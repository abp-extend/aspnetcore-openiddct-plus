using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreOpeniddictPlus.Core.Interfaces;

public interface IAuthorizationService
{
    Task<IActionResult> AuthorizeAsync();
    Task<IActionResult> LogoutAsync();

    Task<IActionResult> ExchangeAsync();

    Task<IActionResult> AcceptAsync();

    IActionResult Deny();

}
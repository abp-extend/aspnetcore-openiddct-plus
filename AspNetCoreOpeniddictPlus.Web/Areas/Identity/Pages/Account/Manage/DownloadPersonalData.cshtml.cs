// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text.Json;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Account.Manage;

public class DownloadPersonalDataModel : PageModel
{
    private readonly ILogger<DownloadPersonalDataModel> _logger;
    private readonly UserManager<OpeniddictPlusUser> _userManager;

    public DownloadPersonalDataModel(
        UserManager<OpeniddictPlusUser> userManager,
        ILogger<DownloadPersonalDataModel> logger
    )
    {
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        return NotFound();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            return Redirect("/Identity/Account/Login");
        }

        _logger.LogInformation(
            "User with ID '{UserId}' asked for their personal data.",
            _userManager.GetUserId(User)
        );

        // Only include personal data for download
        var personalData = new Dictionary<string, string>();
        var personalDataProps = typeof(IdentityUser)
            .GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
        foreach (var p in personalDataProps)
            personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");

        var logins = await _userManager.GetLoginsAsync(user);
        foreach (var l in logins)
            personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);

        personalData.Add("Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));

        Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
        return new FileContentResult(
            JsonSerializer.SerializeToUtf8Bytes(personalData),
            "application/json"
        );
    }
}

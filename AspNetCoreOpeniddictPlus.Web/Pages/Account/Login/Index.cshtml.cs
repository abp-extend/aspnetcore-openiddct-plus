using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Pages.Account.Login;

public class Index(SignInManager<OpeniddictPlusUser> signInManager, ILogger<Index> logger) : PageModel
{

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

 

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
       logger.LogDebug($"Username: {Username}, Password: {Password}");

        if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
           // ModelState.AddModelError("Username", "Invalid Credentials");
            return Page();
        }
        var result = await signInManager.PasswordSignInAsync(Username, Password, false, false);

        if (result.Succeeded)
        {
            return Redirect("~/connect/authorize");
        }

        ModelState.AddModelError("InvalidCredentials", "Invalid login attempt.");
        return Page();
    }
}
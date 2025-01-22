using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Pages;

public class Index(SignInManager<OpeniddictPlusUser> signInManager, ILogger<Index> logger) : PageModel
{
    public IActionResult OnGet()
    {
        return Redirect(signInManager.IsSignedIn(User) ? "/Identity/Account/Manage" : "/Identity/Account/Login");
    }
}
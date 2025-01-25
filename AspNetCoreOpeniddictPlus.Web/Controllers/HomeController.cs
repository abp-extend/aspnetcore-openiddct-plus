using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    UserManager<OpeniddictPlusUser> userManager
) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
            return View();
        logger.LogWarning($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        return Redirect("/Identity/Account/Login");
    }
}

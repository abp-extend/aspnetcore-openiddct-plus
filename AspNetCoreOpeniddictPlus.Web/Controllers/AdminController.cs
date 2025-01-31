using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.InertiaCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

public class AdminController(
    UserManager<OpeniddictPlusUser> userManager,
    IUserService<OpeniddictPlusUser> userService,
    IHttpContextAccessor httpContextAccessor) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        return Inertia.Render("Index", new { name = "Hello World" });
    }
    
    [HttpGet("/admin/user-management")]
    public async Task<IActionResult> UserManagement()
    {
        if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        var user = await userService.GetUsersAsync();
        var data = user.Items;
        var currentPage = user.CurrentPage;
        var totalPages = user.TotalCount;
        var hasNextPage = user.HasNextPage;
        var hasPreviousPage = user.HasPreviousPage;
        var pageSize = user.PageSize;
        return Inertia.Render("UserManagement", new
        {
            userResponse = new
            {
                data, currentPage, totalPages, hasNextPage, hasPreviousPage, pageSize
            }
        });
    }
}
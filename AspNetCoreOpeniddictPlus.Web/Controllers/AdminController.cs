using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.InertiaCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(
    UserManager<OpeniddictPlusUser> userManager,
    RoleManager<OpeniddictPlusRole> roleManager,
    IUserService<OpeniddictPlusUser> userService,
    IHttpContextAccessor httpContextAccessor) : Controller
{
    public async Task<IActionResult> Index()
    {
        return Inertia.Render("Index", new { name = "Hello World" });
    }
    
    [HttpGet("/admin/user-management")]
    public async Task<IActionResult> UserManagement(string? error = null)
    {
        
        var users = await userService.GetUsersAsync();
      
        return Inertia.Render("UserManagement", new
        {
            userResponse = new
            {
                users.Items,
                users.TotalCount,
                users.CurrentPage,
                users.PageSize,
                users.HasPreviousPage,
                users.HasNextPage
            },
            error
        });
    }
    
    [HttpPost("/admin/user-management"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreateUserDto createUserDto)
    {
        var newUser = new OpeniddictPlusUser
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            UserName = createUserDto.UserName,
            CreatedByAdmin = true
        };
        var user = await userManager.CreateAsync(newUser);
        if (!user.Succeeded)
        {
          return await UserManagement("Failed to create user");
        }
        await userManager.AddPasswordAsync(newUser, createUserDto.Password);
        return RedirectToAction("UserManagement");
    }
    
    [HttpPost("/admin/user-management/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromForm] DeleteDto dto)
    {
        var user = await userManager.FindByIdAsync(dto.Id);
        if (user == null)
        {
            return await UserManagement("User not found");
        }

        var role = await userManager.GetRolesAsync(user);
        if (role.Contains("Admin"))
        {
            return await UserManagement("Cannot delete admin user");
        }
        user.DeletionRequestedAt = DateTime.UtcNow;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return await UserManagement("Failed to delete user");
        }
        return RedirectToAction("UserManagement");
    }
    
    [HttpPost("/admin/user-management/update"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromForm] UpdateUserDto dto)
    {
        var user = await userManager.FindByIdAsync(dto.Id);
        if (user == null)
        {
            return await UserManagement("User not found");
        }
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.UserName = dto.UserName;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return await UserManagement("Failed to update user");
        }
        return RedirectToAction("UserManagement");
    }
   
}
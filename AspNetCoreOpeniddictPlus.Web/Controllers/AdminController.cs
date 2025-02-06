using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;

using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.InertiaCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("users")]
public class AdminController(
    UserManager<OpeniddictPlusUser> userManager,
    RoleManager<OpeniddictPlusRole> roleManager,
    IHttpContextAccessor httpContextAccessor) : Controller
{
    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        return Inertia.Render("Index", new { name = "Hello World" });
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> UserManagement(string? error = null, int currentPage = 1, int pageSize = 10)
    {
        
        var users = await userManager
            .Users
            .ToPagedResultAsync(currentPage, pageSize);
      
        var userItems = users.Items.Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            UserName = u.UserName,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt,
            DeletionRequestedAt = u.DeletionRequestedAt,
            CreatedByAdmin = u.CreatedByAdmin,
            EmailConfirmed = u.EmailConfirmed,
            Roles = userManager.GetRolesAsync(u).Result.ToList()
        }).ToList();
        
        return Inertia.Render("UserManagement", new
        {
            data = new
            {
                items = userItems,
                users.TotalCount,
                users.CurrentPage,
                users.PageSize,
                users.HasPreviousPage,
                users.HasNextPage
            },
            error
        });
    }
    
    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreateUserDto createUserDto)
    {
        var newUser = new OpeniddictPlusUser
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            UserName = createUserDto.UserName,
            CreatedByAdmin = true,
            CreatedAt = DateTime.UtcNow
        };
        var user = await userManager.CreateAsync(newUser);
        if (!user.Succeeded)
        {
          return await UserManagement("Failed to create user");
        }
        await userManager.AddPasswordAsync(newUser, createUserDto.Password);
        return RedirectToAction("UserManagement");
    }
    
    [HttpPost("delete"), ValidateAntiForgeryToken]
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
    
    [HttpPost("update"), ValidateAntiForgeryToken]
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
        user.UpdatedAt = DateTime.UtcNow;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return await UserManagement("Failed to update user");
        }
        return RedirectToAction("UserManagement");
    }
   
}


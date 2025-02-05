using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.InertiaCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

[Route("roles")]
public class RoleController(
    RoleManager<OpeniddictPlusRole> roleManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        return Inertia.Render("Index", new { name = "Hello World Role Controller" });
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> RoleManagement(string? error = null, int currentPage = 1, int pageSize = 10)
    {
        var roles = await roleManager
            .Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToPagedResultAsync(currentPage, pageSize);
       
        var result = roles.Items.Select(role => new RolePermissionDto
        {
            RoleId = role.Id,
            RoleName = role.Name,
            Permissions = role.RolePermissions.Select(rp => new PermissionDto
            {
                PermissionId = rp.Permission.Id.ToString(),
                Name = rp.Permission.Name,
                Description = rp.Permission.Description
            }).ToList()
        }).ToList();
        var pagedResult = new PagedResult<RolePermissionDto>
        {
            Items = result,
            TotalCount = roles.TotalCount,
            CurrentPage = roles.CurrentPage,
            PageSize = roles.PageSize,
        };
        return Inertia.Render("RoleManagement", new
        {
            data = new
            {
                pagedResult.Items,
                pagedResult.TotalCount,
                pagedResult.CurrentPage,
                pagedResult.PageSize,
                pagedResult.HasPreviousPage,
                pagedResult.HasNextPage
            },
            error
        });
    }
    
    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] CreateRoleDto createRoleDto)
    {
        var newRole = new OpeniddictPlusRole
        {
            Name = createRoleDto.Name,
            CreatedAt = DateTime.UtcNow
        };
        var user = await roleManager.CreateAsync(newRole);
        if (!user.Succeeded)
        {
            return await RoleManagement("Failed to create user");
        }
        return RedirectToAction("RoleManagement");
    }
    
    [HttpPost("delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromForm] DeleteDto dto)
    {
        var role = await roleManager.FindByIdAsync(dto.Id);
        if (role == null)
        {
            return await RoleManagement("Role not found");
        }

       
        if (role.Name == "Admin")
        {
            return await RoleManagement("Cannot delete admin role");
        }
     
        var result = await roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            return await RoleManagement("Failed to delete role.");
        }
        return RedirectToAction("RoleManagement");
    }
    
    [HttpPost("update"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromForm] UpdateRoleDto dto)
    {
        var role = await roleManager.FindByIdAsync(dto.Id);
        if (role == null)
        {
            return await RoleManagement("Role not found");
        }
        
        role.Name = dto.Name;
        role.UpdatedAt = DateTime.UtcNow;
        var result = await roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            return await RoleManagement("Failed to update user");
        }
        return RedirectToAction("RoleManagement");
    }
    
}
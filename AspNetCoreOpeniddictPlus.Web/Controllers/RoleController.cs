using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.InertiaCore;
using AspNetCoreOpeniddictPlus.Migrator.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

[Route("roles")]
public class RoleController(
    RoleManager<OpeniddictPlusRole> roleManager,
    OpeniddictPlusDbContext dbContext,
    HybridCache cache) : Controller
{
    public async Task<IActionResult> Index()
    {
        return Inertia.Render("Index", new { name = "Hello World Role Controller" });
    }

    [HttpGet("all")]
    public async Task<IActionResult> RoleManagement(string? error = null, int currentPage = 1, int pageSize = 10)
    {
        if (currentPage > 1 || pageSize != 10)
        {
            await cache.RemoveByTagAsync("roles:all");
        }
        var roles = await cache.GetOrCreateAsync("roles:all", async _ => await roleManager
            .Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToPagedResultAsync(currentPage, pageSize), tags: ["roles:all"]);

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
        await cache.RemoveByTagAsync("roles:all");

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
        await cache.RemoveByTagAsync("roles:all");

        return RedirectToAction("RoleManagement");
    }

    [HttpPost("update"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromForm] RolePermissionDto dto)
    {
        var role = await roleManager.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == dto.RoleId);
        if (role == null)
        {
            return await RoleManagement("Role not found");
        }

        if (dto.Permissions.Count == 0)
        {
           dbContext.RemoveRange(role.RolePermissions);
        }

        var existingPermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToHashSet();
        var incomingPermissionIds = dto.Permissions.Select(p => Guid.Parse(p.PermissionId)).ToHashSet();

        // Add new permissions that are not in existing ones
        var permissionsToAdd = incomingPermissionIds.Except(existingPermissionIds);
        foreach (var permissionId in permissionsToAdd)
        {
            role.RolePermissions.Add(new OpeniddictPlusRolePermission
            {
                PermissionId = permissionId,
                RoleId = role.Id
            });
        }
        
        // Remove permissions that are not in incoming ones
        var permissionsToRemove = existingPermissionIds.Except(incomingPermissionIds);
        foreach (var permissionId in permissionsToRemove)
        {
            var rolePermission = role.RolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
            if (rolePermission is not null)
            {
                dbContext.Remove(rolePermission);
            }
        }
        
        role.Name = dto.RoleName;
        role.UpdatedAt = DateTime.UtcNow;

        var result = await roleManager.UpdateAsync(role);
        await dbContext.SaveChangesAsync();

        if (!result.Succeeded)
        {
            return await RoleManagement("Failed to update user");
        }
        await cache.RemoveByTagAsync("roles:all");

        return RedirectToAction("RoleManagement");
    }
}
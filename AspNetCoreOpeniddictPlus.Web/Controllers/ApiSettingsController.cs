using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace AspNetCoreOpeniddictPlus.Web.Controllers;

[ApiController]
[Route("api/settings")]
public class ApiSettingsController(
    UserManager<OpeniddictPlusUser> userManager, 
    RoleManager<OpeniddictPlusRole> roleManager,
    IHttpContextAccessor httpContextAccessor,
    HybridCache cache): Controller
{
    [HttpGet]
    public async Task<IActionResult> GetSettings()
    {
        if(!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) return Ok(new { currentUser = new
        {
            isAuthententicated = false
        }});
        
        var userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var user = await cache.GetOrCreateAsync(userId, async _ =>   userManager
            .Users
            .FirstOrDefault(u => u.Id == userId), tags:[userId]);
        
        if(user is null || string.IsNullOrEmpty(userId)) return NotFound();
        
        var roleNames = await userManager.GetRolesAsync(user);
        var userRoles = await roleManager
            .Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Where(r => roleNames.Contains(r.Name))
            .ToListAsync();

        var roles = userRoles.Select(r => new
        {
            r.Id,
            r.Name,
        }).ToList();

        var permissions = userRoles
            .SelectMany(r => r.RolePermissions)
            .Select(rp => new
            {
                rp.Permission.Id,
                rp.Permission.Name,
                rp.Permission.Description,
                rp.RoleId
            }).ToList();
        
        return Ok(new
        {
            currentUser = new {
                user.Id,
                user.UserName,
                user.Email,
                user.CreatedByAdmin,
                roles,
                permissions,
                isAuthententicated = true
            }
        });
    }
    
    [Authorize(Policy = "View  Policy")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await roleManager
            .Roles
            .Include(r => r.RolePermissions)
            .ToListAsync();
        var result = roles.Select(r => new
        {
            r.Id,
            r.Name,
        }).ToList();
        return Ok(result);
    }
    
}
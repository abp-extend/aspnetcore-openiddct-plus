using AspNetCoreOpeniddictPlus.Core.Services;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreOpeniddictPlus.Web.Permissions;

public class PermissionHandler(
    UserManager<OpeniddictPlusUser> userManager,
    RoleManager<OpeniddictPlusRole> roleManager) : AuthorizationHandler<PermissionRequirement<string>>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement<string> requirement)
    {
        var userId = context.User?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (userId is null)
        {
            context.Fail();
            return;
        }
        
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            context.Fail();
            return;

        }
        var userRoles = await userManager.GetRolesAsync(user);
        if (userRoles.Count == 0)
        {
            context.Fail();
            return;
        }
        
        var permissions = userRoles
            .Select(roleManager.FindByNameAsync)
            .SelectMany(u => u.Result!.RolePermissions
                .Select(openiddictPlusRolePermission => openiddictPlusRolePermission.Permission.Name)).ToList();

        if (!permissions.Contains(requirement.Permission))
        {
            context.Fail();
            return;
        }
        context.Succeed(requirement);
    }
    
}


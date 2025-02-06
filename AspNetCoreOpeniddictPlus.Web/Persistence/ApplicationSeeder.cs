using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Web.Persistence;

public class ApplicationSeeder(
    IServiceProvider serviceProvider)
    : IHostedService
{
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<OpeniddictPlusUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<OpeniddictPlusRole>>();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService<OpeniddictPlusPermission>>();

        await SeedRolesAndUsers(userManager, roleManager);
        await SeedPermissions(permissionService, roleManager);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task SeedPermissions(IPermissionService<OpeniddictPlusPermission> permissionService, RoleManager<OpeniddictPlusRole> roleManager)
    {
        
        var permissions = new[]
        {
            "Create Policy",
            "Update Policy",
            "Delete Policy",
            "View  Policy",
        };
        var queryable = await permissionService.GetPermissions();
        var count = await queryable.CountAsync();
        if (count > 0) return;
        
        foreach (var permission in permissions)
        {
            var entity = await permissionService.GetPermissionByNameAsync(permission);
            if (entity is null)
            {
                await permissionService.CreatePermissionAsync(new OpeniddictPlusPermission { Name = permission });
            }
        }

        var roles = await roleManager.Roles.ToListAsync();
        foreach (var role in roles)
        {
            if (role.Name is null) continue;
            
            foreach (var permission in permissions)
            {
                var p =await permissionService.GetPermissionByNameAsync(permission);
                if (p is null) continue;
         
                role.RolePermissions.Add(new OpeniddictPlusRolePermission
                {
                    PermissionId = p.Id,
                    RoleId = role.Id
                });
                
            }
           
            await roleManager.UpdateAsync(role);
        }
    }
    private static async Task SeedRolesAndUsers(UserManager<OpeniddictPlusUser> userManager, RoleManager<OpeniddictPlusRole> roleManager)
    {
        var roles = new[] { "Admin", "User", "Manager" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new OpeniddictPlusRole{ Name = role });
            }
        }

        var userName = "admin";
        var adminEmail = "admin@openiddictplus.io";
        var adminPassword = "Admin@123";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new OpeniddictPlusUser
            {
                UserName = userName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }    
}
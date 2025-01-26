using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;

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

        await SeedRolesAndUsers(userManager, roleManager);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

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
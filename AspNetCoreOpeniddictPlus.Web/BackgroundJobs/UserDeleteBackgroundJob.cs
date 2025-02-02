using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreOpeniddictPlus.Web.BackgroundJobs;

public class UserDeleteBackgroundJob(IServiceScopeFactory scopeFactory, ILogger<UserDeleteBackgroundJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<OpeniddictPlusUser>>();
                var now = DateTime.UtcNow;

                var usersToDelete = userManager.Users
                    .Where(u => u.DeletionRequestedAt.HasValue && now >= u.DeletionRequestedAt.Value.AddDays(30))
                    .ToList();

                foreach (var user in usersToDelete)
                {
                    logger.LogInformation($"Deleting user: {user.UserName}");
                    await userManager.DeleteAsync(user);
                }
            }

            // Run this every 24 hours
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
    
}
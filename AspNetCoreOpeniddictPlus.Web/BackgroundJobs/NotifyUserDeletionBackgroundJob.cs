using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Web.BackgroundJobs;

public class NotifyUserDeletionBackgroundJob(IServiceScopeFactory scopeFactory, ILogger<NotifyUserDeletionBackgroundJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<OpeniddictPlusUser>>();
                var now = DateTime.UtcNow;

                var users = await userManager.Users
                    .Where(u => u.DeletionRequestedAt.HasValue && now >= u.DeletionRequestedAt.Value.AddDays(25))
                    .ToListAsync(cancellationToken: stoppingToken);
 
                foreach (var u in users)
                {
                    await Task.WhenAny(NotifyUserAsync(u, scope, stoppingToken));
                }
            }

            // Run this every 24 hours
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
    
    private async Task NotifyUserAsync(OpeniddictPlusUser user, IServiceScope scope, CancellationToken stoppingToken)
    {
        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        await emailSender.SendEmailAsync(user.Email, "User Deletion", "Your account will be deleted in 5 days");
    }
}
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace AspNetCoreOpeniddictPlus.Core.Services;

public class EmailSender(ILogger<EmailSender> logger) : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation($"Sending email to {email}: {subject}");
        logger.LogInformation(htmlMessage);
        return Task.CompletedTask;
    }
}
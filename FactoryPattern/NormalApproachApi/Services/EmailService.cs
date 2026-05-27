using NormalApproachApi.Model;

namespace NormalApproachApi.Services;

public class EmailService : INotificationService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(NotificationRequest request)
    {
        _logger.LogInformation("Sending email to {To} with subject '{Subject}'", request.To, request.Subject);
        return Task.CompletedTask;
    }
}

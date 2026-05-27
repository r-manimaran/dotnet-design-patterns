using FactoryPatternApproach.Models;

namespace FactoryPatternApproach.Services;

public class EmailService : INotificationService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public string NotificationType => "email";

    
    public async Task SendAsync(NotificationRequest request)
    {
        _logger.LogInformation($"Sending Email to {request.To}");
        await Task.CompletedTask;
    }
}

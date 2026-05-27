using FactoryPatternApproach.Models;

namespace FactoryPatternApproach.Services;

public class PushNotificationService : INotificationService
{
    private readonly ILogger<PushNotificationService> _logger;

    public PushNotificationService(ILogger<PushNotificationService> logger)
    {
        _logger = logger;
    }
    public string NotificationType => "push";

    public async Task SendAsync(NotificationRequest request)
    {
        _logger.LogInformation($"Sending Push Notification to {request.To}");
        // Push notification logic with Firebase
        await Task.CompletedTask;
    }
}

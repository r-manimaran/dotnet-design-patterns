using NormalApproachApi.Model;

namespace NormalApproachApi.Services;

public class PushNotificationService : INotificationService
{
    private readonly ILogger<PushNotificationService> _logger;

    public PushNotificationService(ILogger<PushNotificationService> logger)
    {
        _logger = logger;
    }
    public Task SendAsync(NotificationRequest request)
    {
        _logger.LogInformation("Sending push notification to {DeviceId} with message '{Message}'", request.Subject, request.Body);
        return Task.CompletedTask;
    }
}
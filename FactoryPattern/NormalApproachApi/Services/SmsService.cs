using NormalApproachApi.Model;

namespace NormalApproachApi.Services;

public class SmsService : INotificationService
{
    private readonly ILogger<SmsService> _logger;

    public SmsService(ILogger<SmsService> logger)
    {
        _logger = logger;
    }
    public Task SendAsync(NotificationRequest request)
    {
        _logger.LogInformation("Sending SMS to {To} with message '{Message}'", request.To, request.Body);
        return Task.CompletedTask;
    }
}

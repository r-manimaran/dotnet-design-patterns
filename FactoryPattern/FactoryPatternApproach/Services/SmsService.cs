using FactoryPatternApproach.Models;

namespace FactoryPatternApproach.Services;

public class SmsService : INotificationService
{
    private readonly ILogger<SmsService> _logger;

    public SmsService(ILogger<SmsService> logger)
    {
        _logger = logger;
    }
    public string NotificationType => "sms";

    public async Task SendAsync(NotificationRequest request)
    {
        _logger.LogInformation($"Sending SMS to {request.To}");
        // SMS sending logic with Twilio
        await Task.CompletedTask;
    }
}

namespace FactoryPatternApproach.Services;

public class NotificationFactory : INotificationFactory
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationFactory> _logger;

    public NotificationFactory(IServiceScopeFactory scopeFactory,
        ILogger<NotificationFactory> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    public INotificationService GetNotificationService(string type)
    {
        var scope = _scopeFactory.CreateScope();
        var services = scope.ServiceProvider.GetServices<INotificationService>();

        var service = services.FirstOrDefault(s => s.NotificationType.Equals(type, StringComparison.OrdinalIgnoreCase));

        if (service == null) 
        {
            _logger.LogWarning($"No notification service found for type:{type}");
            throw new NotSupportedException($"Notification type '{type}' is not supported");
        }
        return service;
    }
}
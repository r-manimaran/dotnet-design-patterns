namespace FactoryPatternApproach.Services;

public sealed class KeyedNotificationFactory(IServiceProvider serviceProvider) : INotificationFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    public INotificationService GetNotificationService(string type)
    {
        var service =_serviceProvider.GetRequiredKeyedService<INotificationService>(type);
        return service;

    }
}

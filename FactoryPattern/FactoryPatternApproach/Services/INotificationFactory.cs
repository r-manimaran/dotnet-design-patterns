namespace FactoryPatternApproach.Services;

public interface INotificationFactory
{
    INotificationService GetNotificationService(string type);
}

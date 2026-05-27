using FactoryPatternApproach.Models;

namespace FactoryPatternApproach.Services;

public interface INotificationService
{
    Task SendAsync(NotificationRequest request);

    string NotificationType { get; }
}

using NormalApproachApi.Model;

namespace NormalApproachApi.Services;

public interface INotificationService
{
    Task SendAsync(NotificationRequest request);
}

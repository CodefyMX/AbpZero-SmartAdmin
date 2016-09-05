using Abp;
using Abp.Domain.Services;
using Cinotam.ModuleZero.Notifications.UsersAppNotifications.Inputs;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.GeneralSubscriber
{
    public interface IAppNotificationsSubscriber : IDomainService
    {
        Task SubscribeToNotification(NotificationSubscriptionInput input);
        Task SubscribeToAllNotifications(NotificationSubscriptionInput input);
        Task UnSubscribeToAllNotifications(NotificationSubscriptionInput input);
        Task UnSubscribeToNotification(NotificationSubscriptionInput input);
        Task<bool> IsSubscribed(UserIdentifier user, string notificationName);
    }
}

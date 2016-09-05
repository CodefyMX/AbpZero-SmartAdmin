using Abp;
using Abp.Notifications;
using Cinotam.ModuleZero.Notifications.Notifications;
using Cinotam.ModuleZero.Notifications.UsersAppNotifications.Inputs;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.GeneralSubscriber
{
    public class AppNotificationsSubscriber : IAppNotificationsSubscriber
    {
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public AppNotificationsSubscriber(INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task SubscribeToNotification(NotificationSubscriptionInput input)
        {
            await _notificationSubscriptionManager.SubscribeAsync(input.UserIdentifier, input.NotificationName, input.EntityIdentifier);
        }

        public async Task SubscribeToAllNotifications(NotificationSubscriptionInput input)
        {
            await _notificationSubscriptionManager.SubscribeAsync(input.UserIdentifier, NotificationNames.UserDeleted);
            await _notificationSubscriptionManager.SubscribeAsync(input.UserIdentifier, NotificationNames.RoleAssigned);
            await _notificationSubscriptionManager.SubscribeAsync(input.UserIdentifier, NotificationNames.UserCreated);
            await _notificationSubscriptionManager.SubscribeAsync(input.UserIdentifier, NotificationNames.RoleCreated);
            await _notificationSubscriptionManager.SubscribeAsync(input.UserIdentifier, NotificationNames.RoleDeleted);
        }

        public async Task UnSubscribeToAllNotifications(NotificationSubscriptionInput input)
        {
            await _notificationSubscriptionManager.UnsubscribeAsync(input.UserIdentifier, NotificationNames.UserDeleted);
            await _notificationSubscriptionManager.UnsubscribeAsync(input.UserIdentifier, NotificationNames.RoleAssigned);
            await _notificationSubscriptionManager.UnsubscribeAsync(input.UserIdentifier, NotificationNames.UserCreated);
            await _notificationSubscriptionManager.UnsubscribeAsync(input.UserIdentifier, NotificationNames.RoleCreated);
            await _notificationSubscriptionManager.UnsubscribeAsync(input.UserIdentifier, NotificationNames.RoleDeleted);
        }

        public async Task UnSubscribeToNotification(NotificationSubscriptionInput input)
        {
            await _notificationSubscriptionManager.UnsubscribeAsync(input.UserIdentifier, input.NotificationName, input.EntityIdentifier);
        }

        public async Task<bool> IsSubscribed(UserIdentifier user, string notificationName)
        {
            var result = await _notificationSubscriptionManager.IsSubscribedAsync(user, notificationName);
            return result;
        }
    }
}

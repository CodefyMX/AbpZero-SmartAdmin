using Abp;
using Abp.Localization;
using Abp.Notifications;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.RolesAppNotifications.Sender
{
    public class RolesAppNotificationsSender : IRolesAppNotificationsSender
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public RolesAppNotificationsSender(INotificationPublisher notificationPublisher, INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task SendRoleCreatedNotification(User user, Role role)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("RoleCreatedByUser", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["roleName"] = role.DisplayName,
                ["user"] = user.FullName
            };
            await _notificationPublisher.PublishAsync(NotificationNames.RoleCreated, dataToSend);
        }

        public async Task SendRoleEditedNotification(User user, Role role)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("RoleEditedByUser", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["roleName"] = role.DisplayName,
                ["user"] = user.FullName
            };
            await _notificationPublisher.PublishAsync(NotificationNames.RoleEdited, dataToSend);
        }

        public async Task SendRoleDeletedNotification(int tenantId, User user, Role role, long[] userIdsInRole)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("RoleDeletedByUser", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["roleName"] = role.DisplayName,
                ["user"] = user.FullName
            };
            await _notificationPublisher.PublishAsync(NotificationNames.RoleDeleted, dataToSend, severity: NotificationSeverity.Warn);

            foreach (var l in userIdsInRole)
            {
                var userIdentifier = new UserIdentifier(tenantId, l);

                if ((!await (_notificationSubscriptionManager.IsSubscribedAsync(userIdentifier, NotificationNames.RoleDeletedForUser))))
                {
                    await _notificationSubscriptionManager.SubscribeAsync(userIdentifier, NotificationNames.RoleDeletedForUser);
                }
            }

            var roleToUserNotificationData = new LocalizableMessageNotificationData(new LocalizableString("RoleDeletedForUser", AbpModuleZeroConsts.LocalizationSourceName));

            await _notificationPublisher.PublishAsync(NotificationNames.RoleDeletedForUser, roleToUserNotificationData, severity: NotificationSeverity.Warn);

        }
    }
}

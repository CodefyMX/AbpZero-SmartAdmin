using Abp;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.Notifications;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.UsersAppNotifications.Sender
{
    public class UsersAppNotificationsSender : DomainService, IUsersAppNotificationsSender
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public UsersAppNotificationsSender(INotificationPublisher notificationPublisher, INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }



        public async Task SendUserDeletedNotification(User currentUser, User userDeleted)
        {

            var dataToSend =
                new LocalizableMessageNotificationData(new LocalizableString("UserDeletedByUser",
                    AbpModuleZeroConsts.LocalizationSourceName))
                { ["userName"] = currentUser.FullName, ["userNameDeleted"] = userDeleted.FullName, ["userDeletedId"] = userDeleted.Id };
            await _notificationPublisher.PublishAsync(NotificationNames.UserDeleted, dataToSend, severity: NotificationSeverity.Warn);


        }

        public async Task SendUserEditedNotification(User currentUser, User userEdited)
        {

            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("UserEditedNotification", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["userName"] = currentUser.FullName,
                ["userNameEdited"] = userEdited.FullName
            };
            await _notificationPublisher.PublishAsync(NotificationNames.UserEdited, dataToSend);


        }

        public async Task SendUserCreatedNotification(User currentUser, User userCreated)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("UserCreatedNotification", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["userName"] = currentUser.FullName,
                ["userNameCreated"] = userCreated.FullName
            };
            await _notificationPublisher.PublishAsync(NotificationNames.UserCreated, dataToSend);


        }

        public async Task SendRoleAssignedNotification(int? currentTenant, User currentUser, User userAssigned)
        {
            var assignedUserIdentifier = new UserIdentifier(currentTenant, userAssigned.Id);
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("RoleAssignedNotification", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["userName"] = currentUser.FullName,
                ["usernameAssigned"] = userAssigned.FullName,
                ["userId"] = userAssigned.Id
            };
            await
                _notificationPublisher.PublishAsync(NotificationNames.RoleAssigned, dataToSend);
            var isRegistered = await
                _notificationSubscriptionManager.IsSubscribedAsync(assignedUserIdentifier
                    , NotificationNames.RoleAssignedToUser);

            if (!isRegistered)
            {
                await _notificationSubscriptionManager.SubscribeAsync(assignedUserIdentifier, NotificationNames.RoleAssignedToUser);

            }
            var dataToSendUser = new LocalizableMessageNotificationData(new LocalizableString("RoleAssignedNotificationToUser", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["userName"] = currentUser.FullName,
                ["usernameAssigned"] = userAssigned.FullName,
                ["userId"] = userAssigned.Id
            };
            await _notificationPublisher.PublishAsync(NotificationNames.RoleAssignedToUser, dataToSendUser, userIds: new[] { assignedUserIdentifier });

        }
    }
}

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
        private readonly UserManager _userManager;
        public UsersAppNotificationsSender(INotificationPublisher notificationPublisher, UserManager userManager, INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationPublisher = notificationPublisher;
            _userManager = userManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }



        public async Task SendUserDeletedNotification(long? abpSessionUserId, string userNameDeleted)
        {
            if (abpSessionUserId != null)
            {
                var user = await _userManager.GetUserByIdAsync(abpSessionUserId.Value);
                var dataToSend =
                    new LocalizableMessageNotificationData(new LocalizableString("UserDeletedByUser",
                        AbpModuleZeroConsts.LocalizationSourceName))
                    { ["userName"] = user.FullName, ["userNameDeleted"] = userNameDeleted };
                await _notificationPublisher.PublishAsync(NotificationNames.UserDeleted, dataToSend, severity: NotificationSeverity.Warn);
            }

        }

        public async Task SendUserEditedNotification(long? abpSessionUserId, string userNameEdited)
        {
            if (abpSessionUserId != null)
            {
                var user = await _userManager.GetUserByIdAsync(abpSessionUserId.Value);
                var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("UserEditedNotification", AbpModuleZeroConsts.LocalizationSourceName))
                {
                    ["userName"] = user.FullName,
                    ["userNameEdited"] = userNameEdited
                };
                await _notificationPublisher.PublishAsync(NotificationNames.UserEdited, dataToSend);

            }
        }

        public async Task SendUserCreatedNotification(long? abpSessionUserId, string userCreated)
        {
            if (abpSessionUserId != null)
            {
                var user = await _userManager.GetUserByIdAsync(abpSessionUserId.Value);
                var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("UserCreatedNotification", AbpModuleZeroConsts.LocalizationSourceName))
                {
                    ["userName"] = user.FullName,
                    ["userNameCreated"] = userCreated
                };
                await _notificationPublisher.PublishAsync(NotificationNames.UserCreated, dataToSend);

            }
        }

        public async Task SendRoleAssignedNotification(int? currentTenant, long? abpSessionUserId, long? assignedUserId, string[] roleAssigned)
        {
            if (abpSessionUserId.HasValue && assignedUserId.HasValue)
            {
                var user = await _userManager.GetUserByIdAsync(abpSessionUserId.Value);
                var assignedUser = await _userManager.GetUserByIdAsync(assignedUserId.Value);
                var assignedUserIdentifier = new UserIdentifier(currentTenant, assignedUserId.Value);
                var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("RoleAssignedNotification", AbpModuleZeroConsts.LocalizationSourceName))
                {
                    ["userName"] = user.FullName,
                    ["usernameAssigned"] = assignedUser.FullName,
                    ["roleAssigned"] = string.Join(",", roleAssigned)
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
                    ["userName"] = user.FullName,
                    ["usernameAssigned"] = assignedUser.FullName,
                    ["roleAssigned"] = string.Join(",", roleAssigned)
                };
                await _notificationPublisher.PublishAsync(NotificationNames.RoleAssignedToUser, dataToSendUser, userIds: new[] { assignedUserIdentifier });
            }
        }
    }
}

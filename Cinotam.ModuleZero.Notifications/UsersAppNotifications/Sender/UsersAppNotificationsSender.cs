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
        private readonly UserManager _userManager;
        public UsersAppNotificationsSender(INotificationPublisher notificationPublisher, UserManager userManager)
        {
            _notificationPublisher = notificationPublisher;
            _userManager = userManager;
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

        public Task SendUserEditedNotification(long? abpSessionUserId)
        {
            return Task.FromResult(0);
        }

        public Task SendUserCreatedNotification(long? abpSessionUserId)
        {
            return Task.FromResult(0);
        }
    }
}

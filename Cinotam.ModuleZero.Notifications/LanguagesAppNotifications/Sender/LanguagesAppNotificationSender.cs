using Abp.Localization;
using Abp.Notifications;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.LanguagesAppNotifications.Sender
{
    public class LanguagesAppNotificationSender : ILanguagesAppNotificationSender
    {
        private readonly INotificationPublisher _notificationPublisher;

        public LanguagesAppNotificationSender(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task SendLanguageCreatedNotification(ApplicationLanguage languageCreated, User user)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("LanguageCreated", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["languageName"] = languageCreated.DisplayName,
                ["user"] = user.FullName,
            };
            await _notificationPublisher.PublishAsync(NotificationNames.LanguageCreated, dataToSend);
        }

        public async Task SendLanguageDeletedNotification(ApplicationLanguage languageDeleted, User user)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("LanguageDeleted", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["languageName"] = languageDeleted.DisplayName,
                ["user"] = user.FullName,
            };
            await _notificationPublisher.PublishAsync(NotificationNames.LanguageDeleted, dataToSend, severity: NotificationSeverity.Warn);
        }
    }
}

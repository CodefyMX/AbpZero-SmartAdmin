using Abp.Localization;
using Abp.Notifications;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;

namespace Cinotam.ModuleZero.Notifications.LanguagesAppNotifications.Sender
{
    public class LanguagesAppNotificationSender : ILanguagesAppNotificationSender
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public LanguagesAppNotificationSender(INotificationPublisher notificationPublisher, INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public void SendLanguageCreatedNotification(ApplicationLanguage languageCreated, User user)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("LanguageCreated", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["languageName"] = languageCreated.DisplayName,
                ["user"] = user.FullName,
            };
            _notificationPublisher.PublishAsync(NotificationNames.LanguageCreated, dataToSend);
        }

        public void SendLanguageDeletedNotification(ApplicationLanguage languageDeleted, User user)
        {
            var dataToSend = new LocalizableMessageNotificationData(new LocalizableString("LanguageDeleted", AbpModuleZeroConsts.LocalizationSourceName))
            {
                ["languageName"] = languageDeleted.DisplayName,
                ["user"] = user.FullName,
            };
            _notificationPublisher.PublishAsync(NotificationNames.LanguageDeleted, dataToSend);
        }
    }
}

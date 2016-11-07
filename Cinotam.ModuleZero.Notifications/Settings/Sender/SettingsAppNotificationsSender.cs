using Abp.Localization;
using Abp.Notifications;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.Settings.Sender
{
    public class SettingsAppNotificationsSender : ISettingsAppNotificationsSender
    {
        private readonly INotificationPublisher _notificationPublisher;

        public SettingsAppNotificationsSender(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task SendSettingsChangedNotification(int? tenantId, User changedBy)
        {
            var dataToSend =
                 new LocalizableMessageNotificationData(new LocalizableString("SettingsChangedByUser",
                     AbpModuleZeroConsts.LocalizationSourceName))
                 { ["userName"] = changedBy.FullName };
            await _notificationPublisher.PublishAsync(NotificationNames.SettingsChanged, dataToSend, severity: NotificationSeverity.Warn);
        }
    }
}

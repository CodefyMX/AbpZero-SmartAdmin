using Abp.Localization;
using Abp.Notifications;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.MultiTenancyNotifications.Sender
{
    public class MultitenancyNotificationSender : IMultitenancyNotificationSender
    {

        private readonly INotificationPublisher _notificationPublisher;

        public MultitenancyNotificationSender(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task SendTenantCreatedNotification(Tenant tenant, User user)
        {
            var message = new LocalizableString("TenantCreated", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationData = new LocalizableMessageNotificationData(message)
            {
                ["tenancyName"] = tenant.Name,
                ["userName"] = user.FullName
            };
            //Notify host
            await _notificationPublisher.PublishAsync(NotificationNames.TenantCreated, notificationData);

        }

        public async Task SendDeletedNotification(Tenant tenant, User user)
        {
            var message = new LocalizableString("TenantDeleted", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationData = new LocalizableMessageNotificationData(message)
            {
                ["tenancyName"] = tenant.Name,
                ["userName"] = user.FullName
            };
            //Notify host
            await _notificationPublisher.PublishAsync(NotificationNames.TenantDeleted, notificationData);
        }

        public async Task SendTenantRestoredNotification(Tenant tenant, User user)
        {
            var message = new LocalizableString("TenantRestored", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationData = new LocalizableMessageNotificationData(message)
            {
                ["tenancyName"] = tenant.Name,
                ["userName"] = user.FullName
            };
            //Notify host
            await _notificationPublisher.PublishAsync(NotificationNames.TenantRestored, notificationData);
        }
    }
}

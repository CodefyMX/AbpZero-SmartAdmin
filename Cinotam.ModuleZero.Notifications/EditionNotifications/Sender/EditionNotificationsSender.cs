using Abp.Application.Editions;
using Abp.Domain.Entities;
using Abp.Localization;
using Abp.Notifications;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.EditionNotifications.Sender
{
    public class EditionNotificationsSender : IEditionNotificationsSender
    {
        private readonly INotificationPublisher _notificationPublisher;

        public EditionNotificationsSender(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public Task SendNotificationEditionCreated(Edition edition, User creator)
        {
            var message = new LocalizableString("EditionCreatedNotification", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationData = new LocalizableMessageNotificationData(message) { ["editionName"] = edition.DisplayName, ["userName"] = creator.FullName };
            return _notificationPublisher.PublishAsync(NotificationNames.EditionCreated, notificationData);
        }

        public async Task SendNotificationEditionDeleted(Edition edition, User deleter)
        {
            //EditionDeletedForTenant
            var entityIdentifier = new EntityIdentifier(typeof(Edition), edition.Id);
            var message = new LocalizableString("EditionDeletedNotification", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationData = new LocalizableMessageNotificationData(message) { ["editionName"] = edition.DisplayName, ["userName"] = deleter.FullName };


            var messageFTenant = new LocalizableString("EditionDeletedForTenant", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationDataFTenant = new LocalizableMessageNotificationData(messageFTenant) { ["editionName"] = edition.DisplayName };

            //Send notification to host
            await _notificationPublisher.PublishAsync(NotificationNames.EditionDeleted, notificationData);

            //Send notification for clients
            await _notificationPublisher.PublishAsync(NotificationNames.EditionDeleted, notificationDataFTenant, entityIdentifier);

        }

        public async Task SendNotificationEditionEdited(Edition edition, User modifier)
        {
            var entityIdentifier = new EntityIdentifier(typeof(Edition), edition.Id);

            var message = new LocalizableString("EditionChanged", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationData = new LocalizableMessageNotificationData(message) { ["editionName"] = edition.DisplayName, ["userName"] = modifier.FullName };


            var messageFTenant = new LocalizableString("YourEditionWasChanged", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationDataFTenant = new LocalizableMessageNotificationData(messageFTenant);

            //Send notification to host
            await _notificationPublisher.PublishAsync(NotificationNames.EditionEdited, notificationData);

            //Send notification to tenants
            await _notificationPublisher.PublishAsync(NotificationNames.EditionEdited, notificationDataFTenant, entityIdentifier);
        }

        public async Task SendNotificationEditionAssigned(Tenant tenant, Edition edition, User modifier)
        {
            var message = new LocalizableString("TenantEditionChanged", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationData = new LocalizableMessageNotificationData(message)
            {
                ["tenantName"] = tenant.Name,
                ["editionName"] = edition.DisplayName,
                ["userName"] = modifier.FullName
            };

            var entityIdentifier = new EntityIdentifier(typeof(Edition), edition.Id);
            var messageFTenant = new LocalizableString("YourEditionWasChanged", AbpModuleZeroConsts.LocalizationSourceName);
            var notificationDataFTenant = new LocalizableMessageNotificationData(messageFTenant);

            //Send notification to host
            await _notificationPublisher.PublishAsync(NotificationNames.EditionEdited, notificationData);
            //Send notification to tenants
            await _notificationPublisher.PublishAsync(NotificationNames.EditionEdited, notificationDataFTenant, entityIdentifier);
        }
    }
}

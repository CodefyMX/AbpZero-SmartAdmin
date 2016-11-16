using Abp;
using Abp.Application.Editions;
using Abp.Domain.Entities;
using Abp.Notifications;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.Notifications.Notifications;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.EditionNotifications.Subscriber
{
    public class EditionNotificationSubscriber : IEditionNotificationSubscriber
    {
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public EditionNotificationSubscriber(INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task SubscribeTenantToEditionChanges(Tenant tenant, Edition edition, User tenantOwner)
        {
            var entityIdentifier = new EntityIdentifier(typeof(Edition), edition.Id);
            var userIdentifier = new UserIdentifier(tenant.Id, tenantOwner.Id);
            await _notificationSubscriptionManager.SubscribeAsync(userIdentifier, NotificationNames.EditionEdited, entityIdentifier);
            await _notificationSubscriptionManager.SubscribeAsync(userIdentifier, NotificationNames.EditionDeleted, entityIdentifier);
            await _notificationSubscriptionManager.SubscribeAsync(userIdentifier, NotificationNames.RoleAssigned, entityIdentifier);
        }

        public async Task UnSubscribeTenantToEditionChanges(Tenant tenant, Edition tenantPrevEdition, User getTenantOwner)
        {
            var entityIdentifier = new EntityIdentifier(typeof(Edition), tenantPrevEdition.Id);
            var userIdentifier = new UserIdentifier(tenant.Id, getTenantOwner.Id);
            await _notificationSubscriptionManager.UnsubscribeAsync(userIdentifier, NotificationNames.EditionEdited, entityIdentifier);
            await _notificationSubscriptionManager.UnsubscribeAsync(userIdentifier, NotificationNames.EditionDeleted, entityIdentifier);
            await _notificationSubscriptionManager.UnsubscribeAsync(userIdentifier, NotificationNames.RoleAssigned, entityIdentifier);
        }
    }
}

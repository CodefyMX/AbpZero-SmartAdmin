using Abp.Application.Editions;
using Cinotam.AbpModuleZero.MultiTenancy;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.MultiTenancyNotifications.Sender
{
    public class MultiTenancyNotificationSender : IMultitenancyNotificationSender
    {
        public Task SendTenantCreatedNotification(Tenant tenant)
        {
            return Task.FromResult(0);
        }

        public Task SendDeletedNotification(Tenant tenant)
        {
            return Task.FromResult(0);
        }

        public Task SendTenantEditionChanged(Tenant tenant, Edition edition)
        {
            return Task.FromResult(0);
        }

        public Task SendTenantFeaturesChanged(Tenant tenant)
        {
            return Task.FromResult(0);
        }

        public Task SendTenantRestoredNotification(Tenant tenant)
        {
            return Task.FromResult(0);
        }
    }
}

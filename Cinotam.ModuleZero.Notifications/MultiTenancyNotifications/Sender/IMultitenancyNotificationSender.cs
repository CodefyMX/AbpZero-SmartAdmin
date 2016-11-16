using Abp.Application.Editions;
using Abp.Domain.Services;
using Cinotam.AbpModuleZero.MultiTenancy;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.MultiTenancyNotifications.Sender
{
    public interface IMultitenancyNotificationSender : IDomainService
    {
        Task SendTenantCreatedNotification(Tenant tenant);
        Task SendDeletedNotification(Tenant tenant);
        Task SendTenantEditionChanged(Tenant tenant, Edition edition);
        Task SendTenantFeaturesChanged(Tenant tenant);
        Task SendTenantRestoredNotification(Tenant tenant);
    }
}

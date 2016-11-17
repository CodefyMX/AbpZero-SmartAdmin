using Abp.Application.Editions;
using Abp.Domain.Services;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.EditionNotifications.Subscriber
{
    public interface IEditionNotificationSubscriber : IDomainService
    {
        Task SubscribeTenantToEditionChanges(Tenant tenant, Edition edition, User tenantOwner);
        Task UnSubscribeTenantToEditionChanges(Tenant tenant, Edition tenantPrevEdition, User getTenantOwner);
    }
}

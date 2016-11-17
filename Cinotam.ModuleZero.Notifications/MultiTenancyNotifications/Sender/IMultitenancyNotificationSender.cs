using Abp.Domain.Services;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.MultiTenancyNotifications.Sender
{
    public interface IMultitenancyNotificationSender : IDomainService
    {
        Task SendTenantCreatedNotification(Tenant tenant, User user);
        Task SendDeletedNotification(Tenant tenant, User user);
        Task SendTenantRestoredNotification(Tenant tenant, User user);
    }
}

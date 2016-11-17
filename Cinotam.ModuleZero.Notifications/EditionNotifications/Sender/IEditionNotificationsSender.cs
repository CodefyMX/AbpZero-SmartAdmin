using Abp.Application.Editions;
using Abp.Domain.Services;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.EditionNotifications.Sender
{
    public interface IEditionNotificationsSender : IDomainService
    {
        Task SendNotificationEditionCreated(Edition edition, User creator);
        Task SendNotificationEditionDeleted(Edition edition, User deleter);
        Task SendNotificationEditionEdited(Edition edition, User modifier);
        Task SendNotificationEditionAssigned(Tenant tenant, Edition edition, User modifier);
    }
}

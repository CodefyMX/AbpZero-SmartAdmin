using Abp.Domain.Services;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Users;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.RolesAppNotifications.Sender
{
    public interface IRolesAppNotificationsSender : IDomainService
    {
        Task SendRoleCreatedNotification(User user, Role role);
        Task SendRoleEditedNotification(User user, Role role);
        Task SendRoleDeletedNotification(int tenantId, User user, Role role, long[] userIdsInRole);
    }
}

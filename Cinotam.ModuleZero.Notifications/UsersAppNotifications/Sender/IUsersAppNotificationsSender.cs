using Abp.Domain.Services;
using Cinotam.AbpModuleZero.Users;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.UsersAppNotifications.Sender
{
    public interface IUsersAppNotificationsSender : IDomainService
    {

        Task SendUserDeletedNotification(User currentUser, User userDeleted);
        Task SendUserEditedNotification(User currentUser, User userEdited);
        Task SendUserCreatedNotification(User currentUser, User userCreated);
        Task SendRoleAssignedNotification(int? currentTenant, User currentUser, User assignedUser);
    }
}

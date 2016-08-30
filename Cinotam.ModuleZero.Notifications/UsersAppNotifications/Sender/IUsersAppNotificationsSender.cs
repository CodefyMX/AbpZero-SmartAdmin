using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.UsersAppNotifications.Sender
{
    public interface IUsersAppNotificationsSender : IDomainService
    {

        Task SendUserDeletedNotification(long? abpSessionUserId, string userNameDeleted);
        Task SendUserEditedNotification(long? abpSessionUserId);
        Task SendUserCreatedNotification(long? abpSessionUserId);
    }
}

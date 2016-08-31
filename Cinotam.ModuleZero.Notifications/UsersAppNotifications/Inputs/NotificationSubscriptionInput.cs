using Abp;
using Abp.Domain.Entities;

namespace Cinotam.ModuleZero.Notifications.UsersAppNotifications.Inputs
{
    public class NotificationSubscriptionInput
    {
        public UserIdentifier UserIdentifier { get; set; }
        public EntityIdentifier EntityIdentifier { get; set; }
        public string NotificationName { get; set; }


    }
}

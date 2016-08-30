using Abp.Notifications;
using Cinotam.ModuleZero.Notifications.Notifications;
using Cinotam.ModuleZero.Notifications.UsersAppNotifications.Inputs;

namespace Cinotam.ModuleZero.Notifications.UsersAppNotifications.Outputs
{
    public class SendNotificationObject
    {
        public string SenderUserName { get; set; }
        public NotificationType.NotificationTypes NotificationType { get; set; }
        public string NotificationName { get; protected set; }
        public NotificationData NotificationData { get; set; }

        public SendNotificationObject(string senderUserName, NotificationData notificationData, NotificationType.NotificationTypes notificationType)
        {
            SenderUserName = senderUserName;
            NotificationData = notificationData;
            NotificationName = NotificationResolver.ResolveNotification(notificationType);
            NotificationType = notificationType;
        }

    }
}

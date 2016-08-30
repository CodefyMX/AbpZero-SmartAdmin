using Cinotam.ModuleZero.Notifications.UsersAppNotifications.Inputs;
using System;

namespace Cinotam.ModuleZero.Notifications.Notifications
{
    public static class NotificationResolver
    {
        public static string ResolveNotification(NotificationType.NotificationTypes notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.NotificationTypes.UserCreated:
                    return NotificationNames.UserCreated;
                case NotificationType.NotificationTypes.UserDeleted:
                    return NotificationNames.UserDeleted;
                case NotificationType.NotificationTypes.RoleCreated:
                    return NotificationNames.RoleCreated;
                case NotificationType.NotificationTypes.RoleDeleted:
                    return NotificationNames.RoleDeleted;
                case NotificationType.NotificationTypes.RoleAssigned:
                    return NotificationNames.RoleAssigned;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null);
            }
        }
    }
}

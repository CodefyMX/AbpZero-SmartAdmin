using Abp.Application.Services.Dto;
using Abp.Notifications;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class NotificationsOutput
    {
        public NotificationsOutput()
        {
            Notifications = new EditableList<UserNotification>();
        }
        public List<UserNotification> Notifications { get; set; }
    }

    public class NotificationOutput : EntityDto<Guid>
    {
        public string Message { get; set; }
        public UserNotificationState State { get; set; }
        public string NotificationName { get; set; }
    }
}

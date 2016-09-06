using Abp.AutoMapper;
using Abp.Notifications;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Notifications.Dto
{
    public class NotificationOutput
    {
        public NotificationOutput()
        {
            Notifications = new EditableList<NotificationDto>();
        }
        public List<NotificationDto> Notifications { get; set; }
    }
    [AutoMapFrom(typeof(TenantNotification))]
    public class NotificationDto
    {
        public DateTime CreationTime { get; set; }
        public NotificationData Data { get; set; }
        public object EntityId { get; set; }
        public Type EntityType { get; set; }
        public string EntityTypeName { get; set; }
        public string NotificationName { get; set; }
        public NotificationSeverity Severity { get; set; }
        public int? TenantId { get; set; }
    }
}

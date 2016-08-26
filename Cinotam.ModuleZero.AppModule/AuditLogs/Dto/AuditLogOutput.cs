using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.AuditLogs.Dto
{
    public class AuditLogOutput
    {
        public IEnumerable<AuditLogDto> AuditLogs { get; set; }
    }
    [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogDto : EntityDto<long>
    {
        public virtual string BrowserInfo { get; set; }
        public virtual string ClientIpAddress { get; set; }
        public virtual string ServiceName { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string Exception { get; set; }
        public virtual int ExecutionDuration { get; set; }
        public virtual DateTime ExecutionTime { get; set; }
        public virtual string MethodName { get; set; }
        public virtual string Parameters { get; set; }
        public virtual long? UserId { get; set; }
        public string UserName { get; set; }
        public string ExecutionTimeString => ExecutionTime.ToShortDateString();
    }
}

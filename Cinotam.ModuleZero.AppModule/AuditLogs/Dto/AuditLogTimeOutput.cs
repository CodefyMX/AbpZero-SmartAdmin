using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;

namespace Cinotam.ModuleZero.AppModule.AuditLogs.Dto
{
    [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogTimeOutput : EntityDto<long>
    {
        public string MethodName { get; set; }
        public int ExecutionDuration { get; set; }
        public int Hour { get; set; }
        public string BrowserInfo { get; set; }
    }
}

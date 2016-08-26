using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.AuditLogs.Dto
{

    public class AuditLogTimeOutput
    {

        public string AvgExecutionTime { get; set; }
        public int TotalRequestsReceived { get; set; }
        public List<AuditLogTimeOutputDto> AuditLogTimeOutputDtos { get; set; }
    }
    [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogTimeOutputDto : EntityDto<long>
    {
        public string MethodName { get; set; }
        public int ExecutionDuration { get; set; }
        public int Hour { get; set; }
        public string BrowserInfo { get; set; }
    }
}

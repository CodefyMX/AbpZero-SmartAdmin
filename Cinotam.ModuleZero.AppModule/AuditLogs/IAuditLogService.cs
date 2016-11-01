using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.AuditLogs.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.AuditLogs
{
    public interface IAuditLogService : IApplicationService
    {
        Task<AuditLogOutput> GetLatestAuditLogOutput();
        Task<ReturnModel<AuditLogDto>> GetAuditLogTable(RequestModel<object> input);
        Task<AuditLogDto> GetAuditLogDetails(long id);
        AuditLogTimeOutput GetAuditLogTimes(int? count = 50, int code = 0, int? tenantId = null);


        Task<AuditLogOutput> GetLatestAuditLogOutputForTenant(long tenantId);
        Task<ReturnModel<AuditLogDto>> GetAuditLogTableForTenant(RequestModel<object> input, int tenantId);
        Task<AuditLogDto> GetAuditLogDetailsForTenant(long id, int tenantId);




        Task<ReturnModel<LogDto>> GetLogsTable(RequestModel<object> requestModel);
    }
}

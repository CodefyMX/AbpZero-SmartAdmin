using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.AuditLogs.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.AuditLogs
{
    public class AuditLogService : CinotamModuleZeroAppServiceBase, IAuditLogService
    {
        private IAuditingStore _auditingStore;
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly UserManager _userManager;
        public AuditLogService(IAuditingStore auditingStore, IRepository<AuditLog, long> auditLogRepository, UserManager userManager)
        {
            _auditingStore = auditingStore;
            _auditLogRepository = auditLogRepository;
            _userManager = userManager;
        }


        public async Task<AuditLogOutput> GetLatestAuditLogOutput()
        {
            var list = new List<AuditLogDto>();
            var logs = _auditLogRepository.GetAll().OrderByDescending(a => a.ExecutionTime).Take(10).ToList();
            foreach (var auditLog in logs)
            {
                var name = "Client";
                if (auditLog.UserId.HasValue)
                {
                    var user = await _userManager.GetUserByIdAsync(auditLog.UserId.Value);
                    name = user.UserName;
                }
                var log = auditLog.MapTo<AuditLogDto>();
                log.UserName = name;
                list.Add(log);
            }
            return new AuditLogOutput()
            {
                AuditLogs = list
            };
        }

        public ReturnModel<AuditLogDto> GetAuditLogTable(RequestModel<object> input)
        {
            int count;
            var query = _auditLogRepository.GetAll();
            var filteredByLength = GenerateTableModel(input, query, "MethodName", out count);
            return new ReturnModel<AuditLogDto>()
            {
                data = filteredByLength.Select(a => a.MapTo<AuditLogDto>()).ToArray(),
                draw = input.draw,
            };
        }
    }
}

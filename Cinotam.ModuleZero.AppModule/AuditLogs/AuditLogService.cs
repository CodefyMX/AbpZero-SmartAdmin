using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.AuditLogs.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.AuditLogs
{
    [DisableAuditing]
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

        public async Task<ReturnModel<AuditLogDto>> GetAuditLogTable(RequestModel<object> input)
        {
            int count;
            var query = _auditLogRepository.GetAll();
            var filteredByLength = GenerateTableModel(input, query, "MethodName", out count);
            return new ReturnModel<AuditLogDto>()
            {
                iTotalDisplayRecords = count,
                recordsTotal = query.Count(),
                recordsFiltered = filteredByLength.Count,
                length = input.length,
                data = await GetModel(filteredByLength),
                draw = input.draw,
            };
        }

        public async Task<AuditLogDto> GetAuditLogDetails(long id)
        {

            var auditLog = await _auditLogRepository.FirstOrDefaultAsync(a => a.Id == id);
            var mapped = auditLog.MapTo<AuditLogDto>();
            mapped.UserName = auditLog.UserId != null ? (await UserManager.GetUserByIdAsync(auditLog.UserId.Value)).UserName : "Client";
            return mapped;
        }

        [WrapResult(false)]
        public List<AuditLogTimeOutput> GetAuditLogTimes()
        {
            var data = _auditLogRepository.GetAll();
            var listOfData = new List<AuditLogTimeOutput>();
            var query = from ex in data
                        orderby ex.ExecutionTime
                        where DbFunctions.TruncateTime(ex.ExecutionTime) == DbFunctions.TruncateTime(DateTime.Now)
                        select ex;
            foreach (var auditLog in query.Take(100))
            {
                listOfData.Add(new AuditLogTimeOutput()
                {
                    BrowserInfo = auditLog.BrowserInfo,
                    ExecutionDuration = auditLog.ExecutionDuration,
                    Id = auditLog.Id,
                    MethodName = auditLog.MethodName,
                    
                });
            }
            return listOfData;
        }

        private async Task<AuditLogDto[]> GetModel(List<AuditLog> filteredByLength)
        {
            var results = filteredByLength.Select(a => a.MapTo<AuditLogDto>()).ToArray();
            foreach (var auditLogDto in results)
            {

                auditLogDto.UserName = auditLogDto.UserId != null ? (await UserManager.GetUserByIdAsync(auditLogDto.UserId.Value)).UserName : "Client";

            }
            return results.ToArray();
        }
    }
}

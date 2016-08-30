using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.AuditLogs.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.AuditLogs
{
    [DisableAuditing]
    [AbpAuthorize(PermissionNames.AuditLogs)]
    public class AuditLogService : CinotamModuleZeroAppServiceBase, IAuditLogService
    {
        private IAuditingStore _auditingStore;
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IRepository<User, long> _usersRepository;
        private readonly UserManager _userManager;
        public AuditLogService(IAuditingStore auditingStore, IRepository<AuditLog, long> auditLogRepository, UserManager userManager, IRepository<User, long> usersRepository)
        {
            _auditingStore = auditingStore;
            _auditLogRepository = auditLogRepository;
            _userManager = userManager;
            _usersRepository = usersRepository;
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

            List<Expression<Func<AuditLog, string>>> searchs = new EditableList<Expression<Func<AuditLog, string>>>();

            searchs.Add(a => a.MethodName);
            searchs.Add(a => a.ClientIpAddress);
            searchs.Add(a => a.BrowserInfo);
            searchs.Add(a => a.ClientName);
            searchs.Add(a => a.Exception);
            searchs.Add(a => a.ServiceName);
            searchs.Add(a => a.CustomData);
            searchs.Add(a => a.Exception);

            var filteredByLength = GenerateTableModel(input, query, searchs, "MethodName", out count);

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
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var auditLog = await _auditLogRepository.FirstOrDefaultAsync(a => a.Id == id);
                var mapped = auditLog.MapTo<AuditLogDto>();
                mapped.UserName = auditLog.UserId != null ? (await UserManager.GetUserByIdAsync(auditLog.UserId.Value)).UserName : "Client";
                return mapped;
            }

        }

        [WrapResult(false)]
        public AuditLogTimeOutput GetAuditLogTimes(int? count)
        {
            var data = _auditLogRepository.GetAll().OrderByDescending(a => a.ExecutionTime);
            var listOfData = new List<AuditLogTimeOutputDto>();
            var query = from ex in data
                        where DbFunctions.TruncateTime(ex.ExecutionTime) == DbFunctions.TruncateTime(DateTime.Now)
                        select ex;
            var inMemoryData = query.Take(count ?? 100).ToList();
            foreach (var auditLog in inMemoryData)
            {
                listOfData.Add(new AuditLogTimeOutputDto()
                {
                    BrowserInfo = auditLog.BrowserInfo,
                    ExecutionDuration = auditLog.ExecutionDuration,
                    Id = auditLog.Id,
                    MethodName = auditLog.MethodName,

                });
            }
            double avg = 0;
            var totalCalls = 0;
            if (data.Any())
            {

                avg = data.Average(a => a.ExecutionDuration);
                totalCalls = data.Count();
            }
            return new AuditLogTimeOutput()
            {
                TotalRequestsReceived = totalCalls,
                AuditLogTimeOutputDtos = listOfData,
                AvgExecutionTime = avg.ToString("##.#")
            };
        }

        private async Task<AuditLogDto[]> GetModel(IEnumerable<AuditLog> filteredByLength)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
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
}

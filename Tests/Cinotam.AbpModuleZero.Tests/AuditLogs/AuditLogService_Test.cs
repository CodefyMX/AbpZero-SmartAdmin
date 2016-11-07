using Cinotam.ModuleZero.AppModule.AuditLogs;
using Cinotam.ModuleZero.AppModule.AuditLogs.Dto;
using Shouldly;
using System.Collections;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.AuditLogs
{
    public class AuditLogService_Test : AbpModuleZeroTestBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogService_Test()
        {
            _auditLogService = Resolve<IAuditLogService>();

            /*
            Task<ReturnModel<AuditLogDto>> GetAuditLogTable(RequestModel<object> input, int tenantId);
            Task<AuditLogDto> GetAuditLogDetails(long id, int tenantId);
            AuditLogTimeOutput GetAuditLogTimes(int? count, int tenantId);
         */

        }

        [Fact]
        public async Task GetLatestAuditLogOutput_Test()
        {
            LoginAsHostAdmin();
            var auditLogs = await _auditLogService.GetLatestAuditLogOutput();

            auditLogs.ShouldNotBeNull();

            auditLogs.AuditLogs.ShouldNotBeNull();

            auditLogs.AuditLogs.ShouldBeAssignableTo<IEnumerable>();

        }
        [Fact]
        public async Task GetAuditLogTable_Test()
        {
            LoginAsHostAdmin();

            var fakeRequest = FakeRequests.FakeRequestHelper<object>.CreateDataTablesFakeRequestModel();

            var autiLogsTable = await _auditLogService.GetAuditLogTable(fakeRequest);

            autiLogsTable.ShouldNotBeNull();

        }

        [Fact]
        public async Task GetAuditLogDetails_Test()
        {
            LoginAsHostAdmin();

            var fakeDetailRequest = await _auditLogService.GetAuditLogDetails(1);

            fakeDetailRequest.ShouldNotBeNull();
        }

        [Fact]

        public void GetAuditLogTimes_Test()
        {
            LoginAsHostAdmin();

            var fakeResult = _auditLogService.GetAuditLogTimes(new AuditLogTimesInput()
            {
                Count = 10
            });

            fakeResult.ShouldNotBeNull();

            fakeResult.AuditLogTimeOutputDtos.ShouldNotBeNull();

            fakeResult.AvgExecutionTime.ShouldBe("");

            fakeResult.TotalRequestsReceived.ShouldNotBeNull();
        }
        #region MultiTenant

        [Fact]
        public async Task GetLatestAuditLogOutput_Tenant_Test()
        {
            if (IsMultiTenancyEnabled)
            {
                LoginAsHostAdmin();
                var auditLogs = await _auditLogService.GetLatestAuditLogOutputForTenant(1);

                auditLogs.ShouldNotBeNull();

                auditLogs.AuditLogs.ShouldNotBeNull();

                auditLogs.AuditLogs.ShouldBeAssignableTo<IEnumerable>();

            }

        }
        [Fact]
        public async Task GetAuditLogTable_Tenant_Test()
        {

            if (IsMultiTenancyEnabled)
            {
                LoginAsHostAdmin();

                var fakeRequest = FakeRequests.FakeRequestHelper<object>.CreateDataTablesFakeRequestModel();

                var autiLogsTable = await _auditLogService.GetAuditLogTableForTenant(fakeRequest, 1);

                autiLogsTable.ShouldNotBeNull();
            }


        }

        [Fact]
        public async Task GetAuditLogDetails_Tenant_Test()
        {

            if (IsMultiTenancyEnabled)
            {
                LoginAsHostAdmin();

                var fakeDetailRequest = await _auditLogService.GetAuditLogDetailsForTenant(1, 1);

                fakeDetailRequest.ShouldNotBeNull();
            }

        }
        #endregion

    }
}

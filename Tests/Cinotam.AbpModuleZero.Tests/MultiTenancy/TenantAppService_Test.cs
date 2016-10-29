using Cinotam.ModuleZero.AppModule.MultiTenancy;
using Cinotam.ModuleZero.AppModule.MultiTenancy.Dto;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.MultiTenancy
{
    public class TenantAppService_Test : AbpModuleZeroTestBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantAppService_Test()
        {
            _tenantAppService = Resolve<ITenantAppService>();
        }

        [Fact]

        public async Task GetEditionsForTenant_Test()
        {
            LoginAsHostAdmin();

            var editionsOutput = await _tenantAppService.GetEditionsForTenant(1);

            editionsOutput.ShouldNotBeNull();

            editionsOutput.Editions.ShouldNotBeNull();

            editionsOutput.Editions.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task SetEditionForTenant_Test()
        {
            LoginAsHostAdmin();

            var editionsOutput = await _tenantAppService.GetEditionsForTenant(1);

            editionsOutput.Editions.Count.ShouldBeGreaterThan(0);

            var defaultEdition = editionsOutput.Editions.First();

            await _tenantAppService.SetTenantEdition(new SetTenantEditionInput()
            {
                EditionId = defaultEdition.Id,
                TenantId = 1
            });


            //Now the edition should be 


            var editionsOutputTest = await _tenantAppService.GetEditionsForTenant(1);

            editionsOutputTest.ShouldNotBeNull();

            editionsOutputTest.Editions.ShouldNotBeNull();

            editionsOutputTest.Editions.Count.ShouldBeGreaterThan(0);

            editionsOutputTest.Editions.First().IsEnabledForTenant.ShouldBeTrue();
        }

    }
}

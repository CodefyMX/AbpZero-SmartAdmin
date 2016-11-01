using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Features.Dto;
using Cinotam.ModuleZero.AppModule.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
        Task<EditionsForTenantOutput> GetEditionsForTenant(int tenantId);
        Task SetFeatureValuesForTenant(CustomEditionInput input);
        Task SetTenantEdition(SetTenantEditionInput input);
        Task<CustomEditionInput> GetFeaturesForTenant(int tenantId);
        Task ResetFeatures(int tenantId);
        ReturnModel<TenantListDto> GetTenantsTable(RequestModel<object> input);

    }
}

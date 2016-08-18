using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Cinotam.ModuleZero.AppModule.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultOutput<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}

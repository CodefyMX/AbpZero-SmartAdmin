using Abp.Domain.Services;

namespace Cinotam.AbpModuleZero.TenantHelpers.TenantHelperAppServiceBase
{
    public interface ITenantHelperService : IDomainService
    {
        void SetCurrentTenantFromUrl();
        bool IsAValidTenancyName(string tenancyName);
    }
}
namespace Cinotam.AbpModuleZero.TenantHelpers.TenantHelperAppServiceBase
{
    public interface ITenantHelperService
    {
        void SetCurrentTenantFromUrl();
        bool IsAValidTenancyName(string tenancyName);
    }
}
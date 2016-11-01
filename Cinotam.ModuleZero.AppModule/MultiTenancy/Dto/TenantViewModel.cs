using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.MultiTenancy;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy.Dto
{
    [AutoMap(typeof(Tenant))]
    public class TenantViewModel : EntityDto<int>
    {
        public string Name { get; set; }
        public string TenancyName { get; set; }
    }
}

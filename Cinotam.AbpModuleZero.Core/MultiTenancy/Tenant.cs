using Abp.MultiTenancy;
using Cinotam.AbpModuleZero.Users;

namespace Cinotam.AbpModuleZero.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
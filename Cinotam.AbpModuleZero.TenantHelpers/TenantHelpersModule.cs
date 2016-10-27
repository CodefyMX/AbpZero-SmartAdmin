using Abp.Modules;
using System.Reflection;

namespace Cinotam.AbpModuleZero.TenantHelpers
{
    public class TenantHelpersModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

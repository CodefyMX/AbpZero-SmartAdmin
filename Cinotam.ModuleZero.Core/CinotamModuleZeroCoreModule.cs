using Abp.Modules;
using Cinotam.AbpModuleZero;
using System.Reflection;

namespace Cinotam.ModuleZero.Core
{
    [DependsOn(typeof(AbpModuleZeroCoreModule))]
    public class CinotamModuleZeroCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

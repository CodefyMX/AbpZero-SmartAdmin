using Abp.Modules;
using System.Reflection;

namespace Cinotam.ModuleZero.Application
{
    public class CinotamModuleZeroApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

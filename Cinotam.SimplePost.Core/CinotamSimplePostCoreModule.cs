using Abp.Modules;
using Cinotam.AbpModuleZero;
using System.Reflection;

namespace Cinotam.SimplePost.Core
{
    [DependsOn(typeof(AbpModuleZeroCoreModule))]
    public class CinotamSimplePostCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

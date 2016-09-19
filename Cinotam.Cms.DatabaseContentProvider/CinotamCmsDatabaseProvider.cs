using Abp.Modules;
using Cinotam.AbpModuleZero;
using System.Reflection;

namespace Cinotam.Cms.DatabaseContentProvider
{
    [DependsOn(typeof(AbpModuleZeroCoreModule))]
    public class CinotamCmsDatabaseProvider : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

using Abp.Modules;
using Cinotam.AbpModuleZero;
using System.Reflection;

namespace Cinotam.Cms.DatabaseTemplateProvider
{
    [DependsOn(typeof(AbpModuleZeroCoreModule))]
    public class CinotamCmsDatabaseTemplateProvider : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

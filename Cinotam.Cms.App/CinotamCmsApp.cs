using Abp.Modules;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.TenantHelpers;
using Cinotam.Cms.Core;
using System.Reflection;

namespace Cinotam.Cms.App
{
    [DependsOn(
        typeof(AbpModuleZeroCoreModule),
        typeof(CinotamCmsCore),
        typeof(TenantHelpersModule)
    )]
    public class CinotamCmsApp : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<CmsMenuProvider>();
        }
    }
}

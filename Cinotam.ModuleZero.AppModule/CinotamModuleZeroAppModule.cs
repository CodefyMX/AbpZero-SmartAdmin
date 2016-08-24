using Abp.AutoMapper;
using Abp.Modules;
using System.Reflection;

namespace Cinotam.ModuleZero.AppModule
{
    [DependsOn(typeof(Cinotam.AbpModuleZero.AbpModuleZeroCoreModule), typeof(AbpAutoMapperModule))]
    public class CinotamModuleZeroAppModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PreInitialize()
        {
            Configuration.Settings.Providers.Add<ModuleZeroSettingsProvider>();
            Configuration.Navigation.Providers.Add<ModuleZeroNavigationProvider>();
        }
    }
}

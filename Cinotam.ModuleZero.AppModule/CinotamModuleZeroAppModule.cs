using Abp.AutoMapper;
using Abp.Modules;
using System.Reflection;
using Cinotam.FileManager;

namespace Cinotam.ModuleZero.AppModule
{
    [DependsOn(typeof(Cinotam.AbpModuleZero.AbpModuleZeroCoreModule), typeof(AbpAutoMapperModule),typeof(CinotamFileManager))]
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

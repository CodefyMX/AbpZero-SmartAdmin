using Abp.AutoMapper;
using Abp.Modules;
using Cinotam.FileManager;
using Cinotam.ModuleZero.MailSender;
using Cinotam.ModuleZero.Notifications;
using Cinotam.TwoFactorSender;
using System.Reflection;

namespace Cinotam.ModuleZero.AppModule
{
    [DependsOn(
        typeof(AbpModuleZero.AbpModuleZeroCoreModule),
        typeof(AbpAutoMapperModule),
         typeof(TwoFactorSenderModule),
        typeof(CinotamFileManager),
        typeof(CinotamModuleZeroNotifications),
        typeof(CinotamModuleZeroMailSender))]
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

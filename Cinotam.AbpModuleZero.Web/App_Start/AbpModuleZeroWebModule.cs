using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using Cinotam.AbpModuleZero.Api;
using Cinotam.AbpModuleZero.TenantHelpers;
using Cinotam.FileManager.Service;
using Cinotam.ModuleZero.AppModule;
using Cinotam.ModuleZero.BackgroundTasks;
using Cinotam.ModuleZero.BackgroundTasks.Workers.ImagePublisher;
using Cinotam.SimplePost.Application;
using Cinotam.TwoFactorSender;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;

namespace Cinotam.AbpModuleZero.Web
{
    [DependsOn(
        typeof(AbpModuleZeroDataModule),
        typeof(AbpModuleZeroWebApiModule),
        typeof(CinotamModuleZeroAppModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpHangfireModule),
        typeof(AbpWebMvcModule),
        typeof(CinotamModuleZeroBackground),
        typeof(CinotamModuleZeroBackground),
        typeof(TwoFactorSenderModule),
        typeof(TenantHelpersModule),
        typeof(CinotamFileManagerService),
        typeof(CinotamSimplePostAppModule))]
    public class AbpModuleZeroWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<AbpModuleZeroNavigationProvider>();

            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            Configuration.BackgroundJobs.UseHangfire(configuration =>
            {
                configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workerManager.Add(IocManager.Resolve<TryToUpdateProfilePictureToCdnService>());
        }
    }
}

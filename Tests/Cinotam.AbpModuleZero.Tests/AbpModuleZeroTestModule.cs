using Abp.Modules;
using Abp.MultiTenancy;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Cinotam.Cms.DatabaseTemplateProvider;
using Cinotam.Cms.FileSystemTemplateProvider;
using Cinotam.ModuleZero.AppModule;
using NSubstitute;

namespace Cinotam.AbpModuleZero.Tests
{
    [DependsOn(
        typeof(AbpModuleZeroDataModule),
        typeof(AbpTestBaseModule), typeof(CinotamModuleZeroAppModule), typeof(CinotamCmsFileSystemTemplateProvider), typeof(CinotamCmsDatabaseTemplateProvider))]
    public class AbpModuleZeroTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Registering fake services

            IocManager.IocContainer.Register(
                Component.For<IAbpZeroDbMigrator>()
                    .UsingFactoryMethod(() => Substitute.For<IAbpZeroDbMigrator>())
                    .LifestyleSingleton()
                );
        }
    }
}

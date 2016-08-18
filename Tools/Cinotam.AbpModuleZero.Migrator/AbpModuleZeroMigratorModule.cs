using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Cinotam.AbpModuleZero.EntityFramework;

namespace Cinotam.AbpModuleZero.Migrator
{
    [DependsOn(typeof(AbpModuleZeroDataModule))]
    public class AbpModuleZeroMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<AbpModuleZeroDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
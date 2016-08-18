using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Cinotam.AbpModuleZero.EntityFramework;

namespace Cinotam.AbpModuleZero
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(AbpModuleZeroCoreModule))]
    public class AbpModuleZeroDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AbpModuleZeroDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

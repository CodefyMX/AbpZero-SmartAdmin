using Abp.Modules;
using Abp.Zero.EntityFramework;
using Cinotam.AbpModuleZero.EntityFramework;
using Cinotam.Cms.DatabaseContentProvider;
using System.Data.Entity;
using System.Reflection;

namespace Cinotam.AbpModuleZero
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(AbpModuleZeroCoreModule), typeof(CinotamCmsDatabaseProvider))]
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

using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Cinotam.AbpModuleZero.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace Cinotam.AbpModuleZero.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<AbpModuleZero.EntityFramework.AbpModuleZeroDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AbpModuleZero";
        }

        protected override void Seed(AbpModuleZero.EntityFramework.AbpModuleZeroDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}

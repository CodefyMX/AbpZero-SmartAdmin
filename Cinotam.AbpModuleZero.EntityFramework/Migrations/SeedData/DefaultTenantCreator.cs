using System.Linq;
using Cinotam.AbpModuleZero.EntityFramework;
using Cinotam.AbpModuleZero.MultiTenancy;

namespace Cinotam.AbpModuleZero.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly AbpModuleZeroDbContext _context;

        public DefaultTenantCreator(AbpModuleZeroDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}

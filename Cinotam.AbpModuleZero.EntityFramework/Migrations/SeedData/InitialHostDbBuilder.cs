using Cinotam.AbpModuleZero.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Cinotam.AbpModuleZero.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly AbpModuleZeroDbContext _context;

        public InitialHostDbBuilder(AbpModuleZeroDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new SampleOrganizationUnitsCreator(_context).Create();
        }
    }
}

using Abp.Organizations;
using Cinotam.AbpModuleZero.EntityFramework;

namespace Cinotam.AbpModuleZero.Migrations.SeedData
{
    public class SampleOrganizationUnitsCreator
    {
        private readonly AbpModuleZeroDbContext _context;

        public SampleOrganizationUnitsCreator(AbpModuleZeroDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var s1 = new OrganizationUnit(null, "Sample1") { Code = OrganizationUnit.CreateCode(1) };
            _context.OrganizationUnits.Add(s1);
            _context.SaveChanges();
            var s1C1 = new OrganizationUnit(null, "Sample1.1", s1.Id) { Code = OrganizationUnit.CreateCode(1, 1) };
            _context.OrganizationUnits.Add(s1C1);
            _context.SaveChanges();
            var s1C2 = new OrganizationUnit(null, "Sample1.2", s1.Id) { Code = OrganizationUnit.CreateCode(1, 2) };
            _context.OrganizationUnits.Add(s1C2);
            _context.SaveChanges();
            var s1C2C1 = new OrganizationUnit(null, "Sample1.2.1", s1C2.Id) { Code = OrganizationUnit.CreateCode(1, 2, 1) };
            _context.OrganizationUnits.Add(s1C2C1);
            _context.SaveChanges();

        }
    }
}

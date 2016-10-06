using Cinotam.AbpModuleZero.Editions;
using Cinotam.AbpModuleZero.EntityFramework;
using System.Linq;

namespace Cinotam.AbpModuleZero.Migrations.SeedData
{
    public class DefaultEditionsCreator
    {
        private readonly AbpModuleZeroDbContext _context;

        public DefaultEditionsCreator(AbpModuleZeroDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new CinotamEdition() { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

                //TODO: Add desired features to the standard edition, if wanted!
            }
        }
    }
}
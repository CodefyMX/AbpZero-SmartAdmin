using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Category
{
    public class CategoryManager : DomainService, ICategoryManager
    {
        private readonly IRepository<DatabaseEntities.Category.Entities.Category> _categoryRepository;

        public CategoryManager(IRepository<DatabaseEntities.Category.Entities.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> AddEditCategory(string categoryName, string inputCategoryDisplayName)
        {
            var categoryWithSameName = _categoryRepository.FirstOrDefault(a => a.Name.Equals(categoryName));
            if (categoryWithSameName != null) return 0;
            var id = await _categoryRepository.InsertOrUpdateAndGetIdAsync(new DatabaseEntities.Category.Entities.Category()
            {
                Name = categoryName,
                DisplayName = inputCategoryDisplayName
            });
            return id;
        }
    }
}

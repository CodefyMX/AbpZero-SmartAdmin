using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Category.Entities;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Category
{
    public class CategoryManager : DomainService, ICategoryManager
    {
        private readonly IRepository<DatabaseEntities.Category.Entities.Category> _categoryRepository;
        private readonly IRepository<CategoryContent> _categoryContentRepository;
        public CategoryManager(IRepository<DatabaseEntities.Category.Entities.Category> categoryRepository, IRepository<CategoryContent> categoryContentRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryContentRepository = categoryContentRepository;
        }

        public async Task<int> AddEditCategory(string categoryName, string inputCategoryDisplayName)
        {
            var categoryWithSameName = _categoryRepository.FirstOrDefault(a => a.Name.Equals(categoryName));
            if (categoryWithSameName != null)
            {
                categoryWithSameName.Name = categoryName;
                categoryWithSameName.DisplayName = inputCategoryDisplayName;
                return categoryWithSameName.Id;
            };
            var id = await _categoryRepository.InsertOrUpdateAndGetIdAsync(new DatabaseEntities.Category.Entities.Category()
            {
                Name = categoryName,
                DisplayName = inputCategoryDisplayName
            });
            return id;
        }

        public async Task<int> AddEditCategoryContent(CategoryContent categoryContent)
        {
            if (categoryContent.Id != 0)
            {
                var categoryContentFound = _categoryContentRepository.FirstOrDefault(a => a.Id == categoryContent.Id);
                categoryContentFound.DisplayText = categoryContent.DisplayText;
                await _categoryContentRepository.InsertOrUpdateAndGetIdAsync(categoryContentFound);
                return categoryContentFound.Id;
            }
            var id = await _categoryContentRepository.InsertOrUpdateAndGetIdAsync(categoryContent);
            return id;
        }
    }
}

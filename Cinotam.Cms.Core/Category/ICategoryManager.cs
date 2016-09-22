using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Category.Entities;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Category
{
    public interface ICategoryManager : IDomainService
    {
        Task<int> AddEditCategory(string categoryName, string inputCategoryDisplayName);
        Task<int> AddEditCategoryContent(CategoryContent categoryContent);
    }
}

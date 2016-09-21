using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Category
{
    public interface ICategoryManager : IDomainService
    {
        Task<int> AddEditCategory(string categoryName, string inputCategoryDisplayName);
    }
}

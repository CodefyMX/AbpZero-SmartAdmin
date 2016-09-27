using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Categories.Dto;
using Cinotam.Cms.App.Pages.Dto;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Categories
{
    public interface ICategoryService : IApplicationService
    {
        Task AddEditCategory(CategoryInput input);
        ReturnModel<CategoryDto> GetCategories(RequestModel<object> requestModel);
        Task<CategoryForEditModel> GetCategoryForEdit(int? id);
        Task RemoveCategory(int categoryId);
    }
}

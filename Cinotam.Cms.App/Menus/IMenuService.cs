using Abp.Application.Services;
using Cinotam.Cms.App.Menus.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Menus
{
    public interface IMenuService : IApplicationService
    {
        Task<MenuOutput> GetMenuForView();
        Task<List<MenuElement>> GetMenuList();
        Task<int> AddMenu(MenuInput input);
        Task<MenuInput> GetMenuForEdit(int? id);
        Task<int> AddMenuSection(MenuInput input);
        Task<MenuInput> GetMenuSectionForEdit(int? id);
        Task<int> AddMenuSectionItem(MenuInputForItem input);
        Task<MenuInputForItem> GetMenuSectionItemForEdit(int? id);
        Task ChangeOrder(List<OrderInput> orderInputs, string discriminator);
        Task<CategorySetModel> GetCategorySetModel(int id);
        Task SetMenuSectionsFromCategories(CategorySetModel input);
        Task UpdateMenuItemsWhenCategoryChange(int pageId, int? oldCategoryId, int newCategoryId);

        Task DeleteMenu(int menuId);

    }
}

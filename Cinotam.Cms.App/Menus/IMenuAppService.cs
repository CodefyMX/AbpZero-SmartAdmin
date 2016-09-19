using Abp.Application.Services;
using Cinotam.Cms.App.Menus.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Menus
{
    public interface IMenuAppService : IApplicationService
    {
        Task AddMenu(MenuInput input);
        Task<MenuInput> GetMenuInputForEdit(int? id);
        Task<MenuContentInput> GetMenuContentInputForEdit(int? id);
        Task<MenuOutput> GetMenu();
        Task<List<MenuDto>> GetMenuList();
        Task AddMenuContent(MenuContentInput input);
    }
}

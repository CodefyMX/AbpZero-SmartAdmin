using Cinotam.Cms.App.Menus.Dto;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Menus
{
    public interface IMenuAppService
    {
        Task AddMenu(MenuInput input);
        Task<MenuOutput> GetMenu();
    }
}

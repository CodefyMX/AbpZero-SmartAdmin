using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Menus;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Menus
{
    public interface IMenuManager : IDomainService
    {
        Task AddMenuAsync(Menu menu);
        Task AddChildAsync(int parent, Menu menu);
        Task MoveAsync(int menu, int parent);
        Task AddMenuContentAsync(MenuContent menuContent);
    }
}

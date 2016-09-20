using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Menus
{
    public interface IMenuManager : IDomainService
    {
        Task AddMenuAsync(Menu menu);
        Task AddMenuContentAsync(MenuContent menuContent);
        Task AddSectionAsync(MenuSection menu);
        Task AddSectionContentAsync(MenuSectionContent menu);

        Task AddMenuItemAsync(MenuSectionItem sectionItem);
        Task AddMenuItemContentAsync(MenuSectionItemContent sectionItemContent);
    }
}

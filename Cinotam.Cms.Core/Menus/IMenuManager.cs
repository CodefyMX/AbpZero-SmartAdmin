using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Menus
{
    public interface IMenuManager : IDomainService
    {
        Task<int> AddMenuAsync(Menu menu);
        Task<int> AddMenuContentAsync(MenuContent menuContent);
        Task<int> AddSectionAsync(MenuSection menu);
        Task<int> AddSectionContentAsync(MenuSectionContent menu);

        Task<int> AddMenuItemAsync(MenuSectionItem sectionItem);
        Task<int> AddMenuItemContentAsync(MenuSectionItemContent sectionItemContent);
        Task Update(Menu menu);
        Task RemoveSection(MenuSection sectionFromMenu);
        void RemoveSectionItemsForPage(int pageId);
        Task SetItemForPage(Page page);
    }
}

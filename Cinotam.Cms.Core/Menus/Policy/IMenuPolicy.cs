using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;

namespace Cinotam.Cms.Core.Menus.Policy
{
    public interface IMenuPolicy : IDomainService
    {
        void ValidateMenu(Menu menu);
        void ValidateMenuContent(MenuContent menuContent);
        void ValidateMenuSection(MenuSection menuSection);
        void ValidateMenuSectionContent(MenuSectionContent menuSectionContent);
        void ValidateMenuItem(MenuSectionItem sectionItem);
        void ValidateMenuItemContent(MenuSectionItemContent sectionItemContent);
    }
}

using Abp.Domain.Repositories;
using Abp.UI;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;
using System.Linq;

namespace Cinotam.Cms.Core.Menus.Policy
{
    public class MenuPolicy : IMenuPolicy
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuContent> _menuContentRepository;
        private readonly IRepository<MenuSection> _menuSectionRepository;
        private readonly IRepository<MenuSectionContent> _menuSectionContentRepository;
        private readonly IRepository<MenuSectionItem> _menuSectionItemRepository;
        private readonly IRepository<MenuSectionItemContent> _menuSectionItemContentRepository;
        public MenuPolicy(IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository, IRepository<MenuSection> menuSectionRepository, IRepository<MenuSectionContent> menuSectionContentRepository, IRepository<MenuSectionItem> menuSectionItemRepository, IRepository<MenuSectionItemContent> menuSectionItemContentRepository)
        {
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
            _menuSectionRepository = menuSectionRepository;
            _menuSectionContentRepository = menuSectionContentRepository;
            _menuSectionItemRepository = menuSectionItemRepository;
            _menuSectionItemContentRepository = menuSectionItemContentRepository;
        }

        public void ValidateMenu(Menu menu)
        {
            if (menu.Id != 0) return;
            var menus = _menuRepository.GetAllList(a => a.MenuName == menu.MenuName);
            if (menus.Any()) throw new UserFriendlyException("RepeatedMenuName");
        }

        public void ValidateMenuContent(MenuContent menuContent)
        {
            if (menuContent.Id != 0) return;
            var result = _menuContentRepository.FirstOrDefault(a => a.Lang == menuContent.Lang && a.Menu.Id == menuContent.Menu.Id);
            if (result == null) return;
            throw new UserFriendlyException("LangExists");
        }

        public void ValidateMenuSection(MenuSection menuSection)
        {
            if (menuSection.Menu == null) throw new UserFriendlyException("InvalidMenu");
            if (menuSection.Id != 0) return;
            var found = _menuSectionRepository.FirstOrDefault(a => a.SectionName == menuSection.SectionName);
            if (found == null) return;
            throw new UserFriendlyException("RepeatedMenuSectionName");
        }

        public void ValidateMenuSectionContent(MenuSectionContent menuSectionContent)
        {
            if (menuSectionContent.MenuSection == null) throw new UserFriendlyException("InvalidMenuSection");
            if (menuSectionContent.Id != 0) return;
            var found = _menuSectionContentRepository.FirstOrDefault(a => a.Lang == menuSectionContent.Lang && a.MenuSection.Id == menuSectionContent.MenuSection.Id);
            if (found == null) return;
            throw new UserFriendlyException("LangExists");
        }

        public void ValidateMenuItem(MenuSectionItem sectionItem)
        {
            if (sectionItem.MenuSection == null) throw new UserFriendlyException("InvalidMenuItemSection");
            if (sectionItem.Id != 0) return;
            var found = _menuSectionItemRepository.FirstOrDefault(a => a.Name == sectionItem.Name);
            if (found == null) return;
            throw new UserFriendlyException("RepeatedMenuItemName");
        }

        public void ValidateMenuItemContent(MenuSectionItemContent sectionItemContent)
        {
            if (sectionItemContent.MenuSectionItem == null) throw new UserFriendlyException("InvalidaMenuItemSection");
            if (sectionItemContent.Id != 0) return;
            var found = _menuSectionItemContentRepository.FirstOrDefault(a => a.Lang == sectionItemContent.Lang && a.MenuSectionItem.Id == sectionItemContent.MenuSectionItem.Id);
            if (found == null) return;
            throw new UserFriendlyException("LangExists");
        }
    }
}

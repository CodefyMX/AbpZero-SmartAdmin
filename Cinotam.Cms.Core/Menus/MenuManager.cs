using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Cinotam.Cms.Core.Menus.Policy;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Menus
{
    public class MenuManager : DomainService, IMenuManager
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuContent> _menuContentRepository;
        private readonly IRepository<MenuSection> _menuSectionRepository;
        private readonly IRepository<MenuSectionContent> _menuSectionContentRepository;
        private readonly IRepository<MenuSectionItem> _menuSectionItemRepository;
        private readonly IRepository<MenuSectionItemContent> _menuSectionItemContentRepository;
        private readonly IMenuPolicy _menuPolicy;
        public MenuManager(IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository, IMenuPolicy menuPolicy, IRepository<MenuSection> menuSectionRepository, IRepository<MenuSectionContent> menuSectionContentRepository, IRepository<MenuSectionItem> menuSectionItemRepository, IRepository<MenuSectionItemContent> menuSectionItemContentRepository)
        {
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
            _menuPolicy = menuPolicy;
            _menuSectionRepository = menuSectionRepository;
            _menuSectionContentRepository = menuSectionContentRepository;
            _menuSectionItemRepository = menuSectionItemRepository;
            _menuSectionItemContentRepository = menuSectionItemContentRepository;
        }

        public async Task<int> AddMenuAsync(Menu menu)
        {
            _menuPolicy.ValidateMenu(menu);
            var id = await _menuRepository.InsertOrUpdateAndGetIdAsync(menu);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }
        public async Task<int> AddMenuContentAsync(MenuContent menuContent)
        {
            //_menuPolicy.ValidateMenuContent(menuContent);
            var id = await _menuContentRepository.InsertOrUpdateAndGetIdAsync(menuContent);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddSectionAsync(MenuSection menuSection)
        {
            var found = _menuSectionRepository.FirstOrDefault(a => a.SectionName == menuSection.SectionName);

            //_menuPolicy.ValidateMenuSection(found);
            if (found != null)
            {
                found.CategoryDiscriminator = menuSection.CategoryDiscriminator;
                found.Menu = menuSection.Menu;
                found.SectionName = menuSection.SectionName;
                return found.Id;
            }

            var id = await _menuSectionRepository.InsertOrUpdateAndGetIdAsync(menuSection);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddSectionContentAsync(MenuSectionContent menu)
        {
            //_menuPolicy.ValidateMenuSectionContent(menu);
            var sectionContentFound =
                            _menuSectionContentRepository.FirstOrDefault(
                                a => a.Lang == menu.Lang && a.SectionId == menu.MenuSection.Id);
            if (sectionContentFound != null)
            {
                sectionContentFound.Lang = menu.Lang;
                sectionContentFound.DisplayText = menu.DisplayText;
                sectionContentFound.MenuSection = menu.MenuSection;
                var idFound = await _menuSectionContentRepository.InsertOrUpdateAndGetIdAsync(sectionContentFound);
                return idFound;
            }
            var id = await _menuSectionContentRepository.InsertOrUpdateAndGetIdAsync(menu);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddMenuItemAsync(MenuSectionItem sectionItem)
        {
            //_menuPolicy.ValidateMenuItem(sectionItem);

            var itemFound = _menuSectionItemRepository.FirstOrDefault(a => a.Name == sectionItem.Name);
            if (itemFound != null)
            {
                itemFound.Name = sectionItem.Name;
                itemFound.MenuSection = sectionItem.MenuSection;
                await _menuSectionItemRepository.InsertOrUpdateAndGetIdAsync(itemFound);
                return itemFound.Id;
            }

            var id = await _menuSectionItemRepository.InsertOrUpdateAndGetIdAsync(sectionItem);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddMenuItemContentAsync(MenuSectionItemContent sectionItemContent)
        {
            //_menuPolicy.ValidateMenuItemContent(sectionItemContent);

            var found =
                _menuSectionItemContentRepository.FirstOrDefault(
                    a => a.Lang == sectionItemContent.Lang && a.SectionItemId == sectionItemContent.MenuSectionItem.Id && a.PageId == sectionItemContent.PageId);

            if (found != null)
            {

                found.Lang = sectionItemContent.Lang;
                found.DisplayText = sectionItemContent.DisplayText;
                found.PageId = sectionItemContent.PageId;
                found.MenuSectionItem = sectionItemContent.MenuSectionItem;
                await _menuSectionItemContentRepository.InsertOrUpdateAndGetIdAsync(found);
                return found.Id;
            }

            var id = await _menuSectionItemContentRepository.InsertOrUpdateAndGetIdAsync(sectionItemContent);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task Update(Menu menu)
        {
            await _menuRepository.UpdateAsync(menu);
        }

        public async Task RemoveSection(MenuSection sectionFromMenu)
        {
            var contents = _menuSectionContentRepository.GetAllList(a => a.SectionId == sectionFromMenu.Id);
            foreach (var menuSectionContent in contents)
            {
                await _menuSectionContentRepository.DeleteAsync(menuSectionContent);
            }
            var sectionItems = _menuSectionItemRepository.GetAllList(a => a.SectionId == sectionFromMenu.Id);
            foreach (var menuSectionItem in sectionItems)
            {
                var itemContents = _menuSectionItemContentRepository.GetAllList(a => a.SectionItemId == menuSectionItem.Id);
                foreach (var itemContent in itemContents)
                {
                    await _menuSectionItemContentRepository.DeleteAsync(itemContent);
                }
                await _menuSectionItemRepository.DeleteAsync(menuSectionItem);
            }
            await _menuSectionRepository.DeleteAsync(sectionFromMenu);
        }
    }
}

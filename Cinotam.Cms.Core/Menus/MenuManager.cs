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
            _menuPolicy.ValidateMenuContent(menuContent);
            var id = await _menuContentRepository.InsertOrUpdateAndGetIdAsync(menuContent);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddSectionAsync(MenuSection menu)
        {
            _menuPolicy.ValidateMenuSection(menu);
            var id = await _menuSectionRepository.InsertOrUpdateAndGetIdAsync(menu);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddSectionContentAsync(MenuSectionContent menu)
        {
            _menuPolicy.ValidateMenuSectionContent(menu);
            var id = await _menuSectionContentRepository.InsertOrUpdateAndGetIdAsync(menu);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddMenuItemAsync(MenuSectionItem sectionItem)
        {
            _menuPolicy.ValidateMenuItem(sectionItem);
            var id = await _menuSectionItemRepository.InsertOrUpdateAndGetIdAsync(sectionItem);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<int> AddMenuItemContentAsync(MenuSectionItemContent sectionItemContent)
        {
            _menuPolicy.ValidateMenuItemContent(sectionItemContent);
            var id = await _menuSectionItemContentRepository.InsertOrUpdateAndGetIdAsync(sectionItemContent);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task Update(Menu menu)
        {
            await _menuRepository.UpdateAsync(menu);
        }
    }
}

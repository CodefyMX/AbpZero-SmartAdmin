using Abp.Domain.Repositories;
using Cinotam.Cms.Core.Menus.Policy;
using Cinotam.Cms.DatabaseEntities.Menus;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Menus
{
    public class MenuManager : IMenuManager
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuContent> _menuContentRepository;
        private readonly IMenuPolicy _menuPolicy;
        public MenuManager(IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository, IMenuPolicy menuPolicy)
        {
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
            _menuPolicy = menuPolicy;
        }

        public async Task AddMenuAsync(Menu menu)
        {
            _menuPolicy.ValidateMenu(menu);
            await _menuRepository.InsertOrUpdateAndGetIdAsync(menu);
        }

        public async Task AddChildAsync(int parent, Menu menu)
        {
            _menuPolicy.ValidateMenu(menu);
            var parentMenu = _menuRepository.GetAllIncluding(a => a.MenuContents).FirstOrDefault(a => a.Id == parent);
            if (parentMenu != null)
            {
                menu.ParentId = parent;
                await _menuRepository.InsertOrUpdateAndGetIdAsync(menu);
            }
        }

        public Task MoveAsync(int menu, int parent)
        {
            throw new NotImplementedException();
        }

        public async Task AddMenuContentAsync(MenuContent menuContent)
        {
            _menuPolicy.ValidateMenuContent(menuContent);
            await _menuContentRepository.InsertOrUpdateAndGetIdAsync(menuContent);

        }
    }
}

using Abp.Domain.Repositories;
using Cinotam.Cms.DatabaseEntities.Menus;
using System;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Menus
{
    public class MenuManager : IMenuManager
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuContent> _menuContentRepository;
        public MenuManager(IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository)
        {
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
        }

        public Task AddMenuAsync(Menu menu)
        {
            return null;
        }

        public Task AddChildAsync(int parent, Menu menu)
        {
            throw new NotImplementedException();
        }

        public Task MoveAsync(int menu, int parent)
        {
            throw new NotImplementedException();
        }

        public Task AddMenuContentAsync(MenuContent menuContent)
        {
            throw new NotImplementedException();
        }
    }
}

using Abp.Domain.Repositories;
using Abp.UI;
using Cinotam.Cms.DatabaseEntities.Menus;
using System.Linq;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;

namespace Cinotam.Cms.Core.Menus.Policy
{
    public class MenuPolicy : IMenuPolicy
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuContent> _menuContentRepository;
        public MenuPolicy(IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository)
        {
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
        }

        public void ValidateMenu(Menu menu)
        {
            if (menu.Id != 0) return;
            var menus = _menuRepository.GetAllList(a => a.MenuName == menu.MenuName);
            if (menus.Any()) throw new UserFriendlyException("RepeatedName");
        }

        public void ValidateMenuContent(MenuContent menuContent)
        {
            if (menuContent.Id != 0) return;
            var result = _menuContentRepository.FirstOrDefault(a => a.Lang == menuContent.Lang && a.Menu.Id == menuContent.Menu.Id);
            if (result == null) return;
            throw new UserFriendlyException("LangExisting");
        }
    }
}

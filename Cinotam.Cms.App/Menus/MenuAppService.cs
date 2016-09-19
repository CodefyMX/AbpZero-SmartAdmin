using Abp.Domain.Repositories;
using Abp.Extensions;
using Cinotam.Cms.App.Menus.Dto;
using Cinotam.Cms.App.Pages;
using Cinotam.Cms.App.Pages.Dto;
using Cinotam.Cms.Core.Menus;
using Cinotam.Cms.DatabaseEntities.Menus;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menu = Cinotam.Cms.DatabaseEntities.Menus.Menu;
using MenuDto = Cinotam.Cms.App.Menus.Dto.MenuDto;

namespace Cinotam.Cms.App.Menus
{
    public class MenuAppService : IMenuAppService
    {
        #region Dependencies

        private readonly IMenuManager _menuManager;
        private readonly IRepository<Page> _pageRepository;
        private readonly IPagesService _pagesService;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuContent> _menuContentRepository;

        #endregion

        #region Ctor

        public MenuAppService(IMenuManager menuManager, IRepository<Page> pageRepository, IPagesService pagesService, IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository)
        {
            _menuManager = menuManager;
            _pageRepository = pageRepository;
            _pagesService = pagesService;
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
        }


        #endregion

        #region Implementations

        public async Task AddMenu(MenuInput input)
        {
            var page = _pageRepository.Get(input.PageId);
            await _menuManager.AddMenuAsync(new Menu()
            {
                Page = page,
                MenuName = input.Name,

            });
        }

        public async Task<MenuInput> GetMenuInputForEdit(int? id)
        {
            if (!id.HasValue) return (await CreateEmptyMenuInput());
            var firstOrDefault = _menuRepository.GetAllIncluding(a => a.Page).FirstOrDefault(a => a.Id == id.Value);
            if (firstOrDefault == null) return (await CreateEmptyMenuInput());
            var menu = firstOrDefault;
            return new MenuInput()
            {
                AvailableMenus = (await GetAvailableMenus()),
                AvaiablePages = (await GetAvailablePages()),
                Id = menu.Id,
                PageId = menu.Page.Id,
                ParentMenuId = menu.ParentId,
            };
        }
        public Task<MenuContentInput> GetMenuContentInputForEdit(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuOutput> GetMenu()
        {
            var menuList = new List<MenuDto>();
            var parents = _menuRepository.GetAllList(a => a.ParentId == null);

            foreach (var parent in parents)
            {
                var contents = await GetMenuContents(parent.Id);
                var menuDto = new MenuDto()
                {
                    Lang = contents.Lang,
                    Id = parent.Id,
                    Title = contents.Text,
                    Url = contents.Url,
                    Childs = await GetChilds(parent)
                };
                menuList.Add(menuDto);
            }
            return new MenuOutput() { MenuDtos = menuList };
        }

        public async Task<List<MenuDto>> GetMenuList()
        {
            var menuList = new List<MenuDto>();
            var menus = await _menuRepository.GetAllListAsync(a => a.ParentId == null);
            foreach (var parent in menus)
            {
                var contents = await GetMenuContents(parent.Id);
                var menuDto = new MenuDto()
                {
                    Lang = contents.Lang,
                    Id = parent.Id,
                    Title = contents.Text.IsNullOrEmpty() ? "NoLang" : contents.Text,
                    Url = contents.Url,
                    Childs = await GetChilds(parent)
                };
                menuList.Add(menuDto);
            }
            return menuList;
        }

        private async Task<List<MenuDto>> GetChilds(Menu parent)
        {
            var menuList = new List<MenuDto>();
            var childs = _menuRepository.GetAllList(a => a.ParentId == parent.Id);
            foreach (var child in childs)
            {
                var contents = await GetMenuContents(child.Id);
                var menuDto = new MenuDto()
                {
                    Lang = contents.Lang,
                    Id = parent.Id,
                    Title = contents.Text.IsNullOrEmpty() ? "NoContent" : contents.Text,
                    Url = contents.Url,
                    Childs = await GetChilds(child)
                };
                menuList.Add(menuDto);
            }
            return menuList;
        }

        private async Task<MenuContentDto> GetMenuContents(int id)
        {
            var content = await _menuContentRepository.FirstOrDefaultAsync(a => a.Menu.Id == id);
            if (content == null) return new MenuContentDto();
            return new MenuContentDto()
            {
                Lang = content.Lang,
                Text = content.Text,
                Url = content.Url
            };
        }

        public Task AddMenuContent(MenuContentInput input)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Helpers
        private async Task<List<MenuDto>> GetAvailableMenus()
        {
            var result = await GetMenu();
            return result.MenuDtos;
        }

        private async Task<MenuInput> CreateEmptyMenuInput()
        {
            return new MenuInput()
            {
                AvaiablePages = (await GetAvailablePages()),
                AvailableMenus = (await GetAvailableMenus())
            };
        }
        private async Task<List<PageDto>> GetAvailablePages()
        {
            var result = await _pagesService.GetPageListDto();
            return result;
        }

        #endregion
    }
}

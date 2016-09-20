using Abp.Domain.Repositories;
using Abp.Localization;
using Cinotam.AbpModuleZero.Extensions;
using Cinotam.Cms.App.Menus.Dto;
using Cinotam.Cms.Core.Menus;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Menus
{
    public class MenuService : CinotamCmsAppServiceBase, IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<MenuContent> _menuContentRepository;
        private readonly IRepository<MenuSection> _menuSectionRepository;
        private readonly IRepository<MenuSectionContent> _menuSectionContentRepository;
        private readonly IRepository<MenuSectionItem> _menuSectionItemRepository;
        private readonly IRepository<MenuSectionItemContent> _menuSectionItemContentRepository;
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IMenuManager _menuManager;
        public MenuService(IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository, IRepository<MenuSection> menuSectionRepository, IRepository<MenuSectionContent> menuSectionContentRepository, IRepository<MenuSectionItem> menuSectionItemRepository, IRepository<MenuSectionItemContent> menuSectionItemContentRepository, IApplicationLanguageManager applicationLanguageManager, IMenuManager menuManager)
        {
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
            _menuSectionRepository = menuSectionRepository;
            _menuSectionContentRepository = menuSectionContentRepository;
            _menuSectionItemRepository = menuSectionItemRepository;
            _menuSectionItemContentRepository = menuSectionItemContentRepository;
            _applicationLanguageManager = applicationLanguageManager;
            _menuManager = menuManager;
        }

        public Task<MenuOutput> GetMenuForView()
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuElement>> GetMenuList()
        {
            var menus = await _menuRepository.GetAllListAsync();
            var elements = new List<MenuElement>();
            foreach (var menu in menus.OrderBy(a => a.Order))
            {

                elements.Add(new MenuElement()
                {
                    DisplayName = menu.MenuName,
                    Id = menu.Id,
                    PageElementContents = await GetContentsForElement(menu.Id)
                });
            }
            return elements;
        }

        private async Task<List<MenuElementContent>> GetContentsForElement(int menuId)
        {
            var contents = _menuContentRepository.GetAllList(a => a.MenuId == menuId);
            var list = new List<MenuElementContent>();
            var allLanguages = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            foreach (var menuContent in contents)
            {
                var coincidence = allLanguages.FirstOrDefault(a => a.Name.Equals(menuContent.Lang));
                if (coincidence == null) continue;
                list.Add(new MenuElementContent()
                {
                    Icon = coincidence.Icon,
                    Lang = coincidence.Name
                });
            }
            return list;
        }



        private async Task<List<MenuElementContent>> CreateEmptyLanguagesOptions()
        {
            var list = new List<MenuElementContent>();
            var allLanguages = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            foreach (var language in allLanguages)
            {
                list.Add(new MenuElementContent()
                {
                    Icon = language.Icon,
                    Lang = language.Name
                });
            }
            return list;
        }

        private MenuElement EmptyLangMenuElement(string lang)
        {
            return new MenuElement()
            {
                DisplayName = L("NoAvailableLangText"),
                Id = 0,
                Lang = lang
            };
        }

        public async Task<int> AddMenu(MenuInput input)
        {
            int id;
            Menu menu;
            if (input.Id == 0)
            {
                menu = new Menu()
                {
                    MenuName = input.Name.Sluggify(),
                    IsActive = input.IsActive,
                    Id = input.Id,
                    Order = _menuRepository.Count() + 1
                };
                id = await _menuManager.AddMenuAsync(menu);
            }
            else
            {
                menu = _menuRepository.Get(input.Id);
                menu.IsActive = input.IsActive;
                menu.MenuName = input.Name.Sluggify();
                id = await _menuManager.AddMenuAsync(menu);
            }

            foreach (var inputAvailableLang in input.AvailableLangs)
            {
                if (string.IsNullOrEmpty(inputAvailableLang.DisplayText)) continue;
                if (inputAvailableLang.Id == 0)
                {
                    await _menuManager.AddMenuContentAsync(MenuContent.CreateMenuContent(inputAvailableLang.Lang, inputAvailableLang.DisplayText, "", menu));
                }
                else
                {
                    var langContent = _menuContentRepository.Get(inputAvailableLang.Id);
                    langContent.Text = inputAvailableLang.DisplayText;
                    await _menuManager.AddMenuContentAsync(langContent);
                }

            }
            return id;

        }

        public async Task<MenuInput> GetMenuForEdit(int? id)
        {
            if (!id.HasValue) return new MenuInput() { AvailableLangs = await GetEmptyAvailableLangs() };
            var menu = _menuRepository.FirstOrDefault(a => a.Id == id);
            return new MenuInput()
            {
                Name = menu.MenuName,
                Id = menu.Id,
                AvailableLangs = await GetAvailableContents(menu),
                IsActive = menu.IsActive
            };
        }

        private async Task<List<LangInput>> GetAvailableContents(Menu menu)
        {
            var list = new List<LangInput>();
            var allLangs = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            foreach (var applicationLanguage in allLangs)
            {
                var content = _menuContentRepository.FirstOrDefault(a => a.Lang == applicationLanguage.Name && a.MenuId == menu.Id);
                if (content == null)
                {
                    list.Add(new LangInput()
                    {
                        DisplayText = "",
                        Lang = applicationLanguage.Name,
                        Icon = applicationLanguage.Icon,
                    });
                }
                else
                {
                    list.Add(new LangInput()
                    {
                        DisplayText = content.Text,
                        Icon = applicationLanguage.Icon,
                        Lang = applicationLanguage.Name,
                        Id = content.Id
                    });
                }
            }
            return list;
        }

        private async Task<List<LangInput>> GetEmptyAvailableLangs()
        {
            var langs = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            return langs.Select(a => new LangInput()
            {
                DisplayText = "",
                Lang = a.Name,
                Icon = a.Icon
            }).ToList();
        }

        public async Task<int> AddMenuSection(MenuInput input)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuInput> GetMenuSectionForEdit(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddMenuSectionItem(MenuInputForItem input)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuInputForItem> GetMenuSectionItemForEdit(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task ChangeOrder(List<OrderInput> orderInputs, string discriminator)
        {
            switch (discriminator)
            {
                case "Menu":
                    await OrderForMenus(orderInputs);
                    break;
                case "Section":
                    break;
                case "Item":
                    break;
                default:
                    throw new IndexOutOfRangeException(nameof(discriminator));
            }
        }

        private async Task OrderForMenus(List<OrderInput> orderInputs)
        {
            foreach (var element in orderInputs)
            {
                var menu = _menuRepository.FirstOrDefault(a => a.Id == element.Id);
                if (menu == null) continue;
                menu.Order = element.Order;
                await _menuManager.Update(menu);
            }
        }
    }
}

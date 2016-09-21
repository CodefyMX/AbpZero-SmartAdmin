using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.UI;
using Cinotam.AbpModuleZero.Extensions;
using Cinotam.Cms.App.Menus.Dto;
using Cinotam.Cms.Core.Menus;
using Cinotam.Cms.DatabaseEntities.Category.Entities;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menu = Cinotam.Cms.DatabaseEntities.Menus.Entities.Menu;

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
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Content> _pageContentRepository;
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<CategoryContent> _categoryContentRepository;
        private readonly IMenuManager _menuManager;
        public MenuService(IRepository<Menu> menuRepository, IRepository<MenuContent> menuContentRepository, IRepository<MenuSection> menuSectionRepository, IRepository<MenuSectionContent> menuSectionContentRepository, IRepository<MenuSectionItem> menuSectionItemRepository, IRepository<MenuSectionItemContent> menuSectionItemContentRepository, IApplicationLanguageManager applicationLanguageManager, IMenuManager menuManager, IRepository<Category> categoryRepository, IRepository<CategoryContent> categoryContentRepository, IRepository<Page> pageRepository, IRepository<Content> pageContentRepository)
        {
            _menuRepository = menuRepository;
            _menuContentRepository = menuContentRepository;
            _menuSectionRepository = menuSectionRepository;
            _menuSectionContentRepository = menuSectionContentRepository;
            _menuSectionItemRepository = menuSectionItemRepository;
            _menuSectionItemContentRepository = menuSectionItemContentRepository;
            _applicationLanguageManager = applicationLanguageManager;
            _menuManager = menuManager;
            _categoryRepository = categoryRepository;
            _categoryContentRepository = categoryContentRepository;
            _pageRepository = pageRepository;
            _pageContentRepository = pageContentRepository;
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

        public async Task<CategorySetModel> GetCategorySetModel(int id)
        {
            var availableCategories = await _categoryRepository.GetAllListAsync();
            var availableCategoriesList = new List<CategorySetInputModel>();
            foreach (var availableCategory in availableCategories)
            {
                string isInCategoryName;
                if (IsThisCategoryInAnotherMenu(availableCategory, out isInCategoryName))
                {
                    availableCategoriesList.Add(new CategorySetInputModel()
                    {
                        CategoryDisplayName = availableCategory.DisplayName,
                        CategoryId = availableCategory.Id,
                        Enabled = false,
                        NameOfCategoryIsIn = isInCategoryName,
                        Checked = IsInThisMenu(id, availableCategory)
                    });
                }
                else
                {
                    availableCategoriesList.Add(new CategorySetInputModel()
                    {
                        CategoryDisplayName = availableCategory.DisplayName,
                        CategoryId = availableCategory.Id,
                        Enabled = true,
                        NameOfCategoryIsIn = isInCategoryName,
                    });
                }
            }
            return new CategorySetModel()
            {
                MenuId = id,
                AvailableCategories = availableCategoriesList
            };
        }

        public async Task SetMenuSectionsFromCategories(CategorySetModel input)
        {
            var menu = _menuRepository.FirstOrDefault(input.MenuId);
            foreach (var inputAvailableCategory in input.AvailableCategories.Where(a => a.Checked))
            {
                //GetCategory
                var category = _categoryRepository.FirstOrDefault(a => a.Id == inputAvailableCategory.CategoryId);

                //GetCategoryContent
                var contents =
                    _categoryContentRepository.GetAllList(a => a.CategoryId == inputAvailableCategory.CategoryId);

                //Create a new section with the name of the category
                var section = MenuSection.CreateMenuSection(category.Name, menu);
                await _menuManager.AddSectionAsync(section);
                //If the category has no contents throw an error
                if (!contents.Any()) throw new UserFriendlyException(L("CategoryHasNoContent"));


                //Now we add the translations for each category (the categories are going to be converted to sections)
                foreach (var categoryContent in contents)
                {
                    var sectionContent = MenuSectionContent.CreateMenuSectionContent(categoryContent.Lang,
                        categoryContent.DisplayText, section);
                    await _menuManager.AddSectionContentAsync(sectionContent);
                }

                //Now we get each page in the category
                var pagesInCategory =
                    _pageRepository.GetAllList(a => a.IncludeInMenu && a.Active && a.CategoryId == category.Id);

                //And convert those page into links for the menu
                foreach (var page in pagesInCategory)
                {
                    var pageContents = _pageContentRepository.GetAllList(a => a.PageId == page.Id);

                    if (!pageContents.Any()) { throw new UserFriendlyException(L("PageHasNoContent")); }

                    var menuSectionItem = MenuSectionItem.CreateMenuSectionItem(page.Name, section);

                    await _menuManager.AddMenuItemAsync(menuSectionItem);

                    foreach (var pageContent in pageContents)
                    {
                        var menuSectionItemContent =
                            MenuSectionItemContent.CreateMenuSectionItemContent(pageContent.Title, pageContent.Lang,
                                menuSectionItem);
                        await _menuManager.AddMenuItemContentAsync(menuSectionItemContent);
                    }
                }

            }


        }
        private bool IsInThisMenu(int id, Category availableCategory)
        {
            var sections = _menuSectionRepository.GetAllList(a => a.CategoryDiscriminator == availableCategory.Name && a.MenuId == id);
            return sections.Any();
        }

        private bool IsThisCategoryInAnotherMenu(Category availableCategory, out string isInCategoryName)
        {
            var isFound = _menuSectionRepository.FirstOrDefault(a => a.CategoryDiscriminator == availableCategory.Name);
            if (isFound == null)
            {
                isInCategoryName = string.Empty;
                return false;

            }
            isInCategoryName = isFound.SectionName;
            return true;
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


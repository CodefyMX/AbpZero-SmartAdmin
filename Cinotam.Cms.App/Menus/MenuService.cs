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
using System.Globalization;
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
        private const string DynamicCreated = "DynamicCreated";
        public MenuService(IRepository<Menu> menuRepository,
            IRepository<MenuContent> menuContentRepository,
            IRepository<MenuSection> menuSectionRepository,
            IRepository<MenuSectionContent> menuSectionContentRepository,
            IRepository<MenuSectionItem> menuSectionItemRepository,
            IRepository<MenuSectionItemContent> menuSectionItemContentRepository,
            IApplicationLanguageManager applicationLanguageManager,
            IMenuManager menuManager,
            IRepository<Category> categoryRepository,
            IRepository<CategoryContent> categoryContentRepository,
            IRepository<Page> pageRepository,
            IRepository<Content> pageContentRepository)
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
        public async Task<MenuOutput> GetMenuForView()
        {
            var allMenus = _menuRepository.GetAllList(a => a.IsActive && a.MenuContents.Any()).OrderBy(a => a.Order);
            var menuElements = new List<MenuElement>();
            foreach (var allMenu in allMenus)
            {
                var contentFromMenu = await GetContentForCurrentLanguage(allMenu.Id);
                if (contentFromMenu == null) continue;
                menuElements.Add(new MenuElement()
                {
                    DisplayName = contentFromMenu.DisplayName,
                    Lang = contentFromMenu.Lang,
                    Id = allMenu.Id,
                    SectionElements = await GetMenuSections(allMenu.Id)
                });
            }
            return new MenuOutput()
            {
                MenuElements = menuElements
            };
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


        public Task<int> AddMenuSection(MenuInput input)
        {
            throw new NotImplementedException();
        }

        public Task<MenuInput> GetMenuSectionForEdit(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddMenuSectionItem(MenuInputForItem input)
        {
            throw new NotImplementedException();
        }

        public Task<MenuInputForItem> GetMenuSectionItemForEdit(int? id)
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
                string isInMenuName;
                int idOfMenuIsIn;
                if (IsThisCategoryInAnotherMenu(availableCategory, out isInMenuName, out idOfMenuIsIn))
                {
                    availableCategoriesList.Add(new CategorySetInputModel()
                    {
                        CategoryDisplayName = availableCategory.DisplayName,
                        CategoryId = availableCategory.Id,
                        Enabled = false,
                        NameOfMenuIsIn = isInMenuName,
                        IdMenuIsIn = idOfMenuIsIn,
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
                        NameOfMenuIsIn = isInMenuName,
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
            foreach (var inputAvailableCategory in input.AvailableCategories)
            {
                if (inputAvailableCategory.Checked)
                {
                    //GetCategory
                    var category = _categoryRepository.FirstOrDefault(a => a.Id == inputAvailableCategory.CategoryId);

                    //GetCategoryContent
                    var contents =
                        _categoryContentRepository.GetAllList(a => a.CategoryId == inputAvailableCategory.CategoryId);

                    //If the category has no contents throw an error
                    if (!contents.Any()) throw new UserFriendlyException(L("CategoryHasNoContent"));

                    //Create a new section with the name of the category
                    var section = await CreateMenuSection(category, menu);

                    //Now we add the translations for each category (the categories are going to be converted to sections)

                    await CreateSectionContents(contents, section);

                    //Now we get each page in the category and set the menu items
                    await SetMenuItemsFromPagesInCategory(category, section);
                }
                else
                {
                    await RemoveItemsFromCategory(inputAvailableCategory.CategoryId);
                }
            }
        }

        public async Task UpdateMenuItemsWhenCategoryChange(int pageId, int? oldCategoryId, int newCategoryId)
        {
            var section = _menuSectionRepository.FirstOrDefault(a => a.CategoryId == oldCategoryId) ??
                          _menuSectionRepository.FirstOrDefault(a => a.CategoryId == newCategoryId);

            if (section == null) return;

            var elementFromOldSection = await _menuSectionItemRepository.FirstOrDefaultAsync(a => a.SectionId == section.Id && a.PageId == pageId);


            var newSection = _menuSectionRepository.FirstOrDefault(a => a.CategoryId == newCategoryId);
            if (newSection == null) return;
            if (elementFromOldSection == null)
            {
                var page = _pageRepository.FirstOrDefault(pageId);

                var newSectionItem = MenuSectionItem.CreateMenuSectionItem(page.Name, newSection, pageId);
                var newSectionEntity = await CreateSectionItem(page, newSection, page.Contents.ToList());
                if (newSectionItem.Id == 0)
                {
                    newSectionItem = _menuSectionItemRepository.FirstOrDefault(newSectionEntity.Id);
                }
                elementFromOldSection = newSectionItem;
            }
            elementFromOldSection.SectionId = newSection.Id;

        }

        public async Task DeleteMenu(int menuId)
        {
            var menu = await _menuRepository.GetAsync(menuId);
            await RemoveSectionsAndContentsFromMenu(menu);
            _menuRepository.Delete(menu);
        }

        private async Task RemoveSectionsAndContentsFromMenu(Menu menu)
        {
            var sections = await _menuSectionRepository.GetAllListAsync(a => a.MenuId == menu.Id);
            foreach (var sectionItem in sections)
            {

                await RemoveSectionItemsFromSection(sectionItem);

                await _menuSectionRepository.DeleteAsync(sectionItem);

            }
        }

        private async Task RemoveSectionItemsFromSection(MenuSection section)
        {
            var sectionItems = await _menuSectionItemRepository.GetAllListAsync(a => a.SectionId == section.Id);
            foreach (var menuSectionItem in sectionItems)
            {
                await _menuSectionItemRepository.DeleteAsync(menuSectionItem);
            }
        }

        #region Helpers
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
        private async Task<List<SectionElement>> GetMenuSections(int allMenuId)
        {
            var sections = await _menuSectionRepository.GetAllListAsync(a => a.MenuId == allMenuId);
            var sectionList = new List<SectionElement>();
            foreach (var menuSection in sections)
            {
                if (menuSection.CategoryId.HasValue)
                {
                    var contentFromCategory = await GetContentFromCategory(menuSection.CategoryId.Value);
                    if (contentFromCategory == null) continue;
                    sectionList.Add(new SectionElement()
                    {
                        DisplayName = contentFromCategory.DisplayName,
                        Lang = contentFromCategory.Lang,
                        SectionItems = await GetSectionItemsFromPages(menuSection.Id)
                    });
                }
                else
                {
                    var contentForSection = await GetContentsForSection(menuSection.Id);
                    if (contentForSection == null) continue;
                    sectionList.Add(new SectionElement()
                    {
                        DisplayName = contentForSection.DisplayName,
                        Lang = contentForSection.Lang,
                        SectionItems = await GetSectionItems(menuSection.Id)
                    });
                }

            }
            return sectionList;
        }

        private async Task<List<SectionItem>> GetSectionItemsFromPages(int menuSectionId)
        {
            var sectionItemContents = new List<SectionItem>();
            var sectionItems = await _menuSectionItemRepository.GetAllListAsync(a => a.SectionId == menuSectionId);
            foreach (var menuSectionItem in sectionItems)
            {
                if (menuSectionItem.PageId.HasValue)
                {
                    var content =
                        _pageContentRepository.FirstOrDefault(
                            a => a.PageId == menuSectionItem.PageId.Value && a.Lang == CultureInfo.CurrentUICulture.Name);
                    if (content == null) continue;
                    sectionItemContents.Add(new SectionItem()
                    {
                        DisplayName = content.Title,
                        HasPage = true,
                        Lang = content.Lang,
                        Url = content.Url
                    });
                }
                else
                {
                    var content = await GetContentForMenuItem(menuSectionItem.Id);
                    sectionItemContents.Add(content);
                }
            }
            return sectionItemContents;

        }

        private async Task<MenuItemContent> GetContentFromCategory(int categoryIdValue)
        {
            var content =
                await _categoryContentRepository.FirstOrDefaultAsync(
                    a => a.CategoryId == categoryIdValue && a.Lang == CultureInfo.CurrentUICulture.Name);
            if (content == null) return null;
            return new MenuItemContent()
            {
                Lang = content.Lang,
                DisplayName = content.DisplayText
            };
        }

        private async Task<List<SectionItem>> GetSectionItems(int menuSectionId)
        {
            var items = await _menuSectionItemRepository.GetAllListAsync(a => a.SectionId == menuSectionId);
            var sectionItems = new List<SectionItem>();
            foreach (var menuSectionItem in items)
            {
                var content = await GetContentForMenuItem(menuSectionItem.Id);
                sectionItems.Add(content);

            }
            return sectionItems;
        }

        private async Task<SectionItem> GetContentForMenuItem(int id)
        {
            var content =
                await _menuSectionItemContentRepository.FirstOrDefaultAsync(
                    a => a.SectionItemId == id && a.Lang == CultureInfo.CurrentUICulture.Name);
            if (content == null) return null;
            return new SectionItem()
            {
                Url = content.PageId.HasValue ? GetPageUrl(content.PageId) : content.Url,
                DisplayName = content.DisplayText,
                HasPage = content.PageId.HasValue,
                Lang = content.Lang,
                Id = content.Id
            };
        }

        private string GetPageUrl(int? contentPageId)
        {
            var pageContent = _pageContentRepository.FirstOrDefault(a => a.PageId == contentPageId.Value);
            return pageContent == null ? null : pageContent.Url;
        }

        private async Task<MenuItemContent> GetContentsForSection(int menuSectionId)
        {
            var foundContent =
                await _menuSectionContentRepository.FirstOrDefaultAsync(
                    a => a.SectionId == menuSectionId && CultureInfo.CurrentUICulture.Name == a.Lang);
            if (foundContent == null) return null;
            return new MenuElement()
            {
                DisplayName = foundContent.DisplayText,
                Lang = foundContent.Lang,
                Id = foundContent.Id
            };
        }

        private async Task<MenuItemContent> GetContentForCurrentLanguage(int allMenuId)
        {
            var content =
                await _menuContentRepository.FirstOrDefaultAsync(
                    a => a.MenuId == allMenuId && a.Lang == CultureInfo.CurrentUICulture.Name);
            if (content == null) return null;
            return new MenuItemContent()
            {
                Lang = content.Lang,
                DisplayName = content.Text
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
        private bool IsInThisMenu(int id, Category availableCategory)
        {
            var sections = _menuSectionRepository.GetAllList(a => a.CategoryId == availableCategory.Id && a.MenuId == id);
            return sections.Any();
        }

        private bool IsThisCategoryInAnotherMenu(Category availableCategory, out string isInCategoryName, out int idCategoryIsIn)
        {
            var isFound = _menuSectionRepository.GetAllIncluding(a => a.Menu).FirstOrDefault(a => a.CategoryId == availableCategory.Id);
            if (isFound == null)
            {
                isInCategoryName = string.Empty;
                idCategoryIsIn = 0;
                return false;
            }
            isInCategoryName = isFound.Menu.MenuName;
            idCategoryIsIn = isFound.MenuId;
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
        private async Task RemoveItemsFromCategory(int categoryId)
        {
            //Remove links for not checked elements
            var categoryForRemove =
            _categoryRepository.FirstOrDefault(a => a.Id == categoryId);

            var sectionFromMenu =
                _menuSectionRepository.FirstOrDefault(a => a.CategoryId == categoryForRemove.Id);
            if (sectionFromMenu != null) await _menuManager.RemoveSection(sectionFromMenu);
        }

        private async Task SetMenuItemsFromPagesInCategory(Category category, MenuSection section)
        {
            var pagesInCategory =
                _pageRepository.GetAllList(a => a.IncludeInMenu && a.Active && a.CategoryId == category.Id);
            //And convert those page into links for the menu
            foreach (var page in pagesInCategory)
            {
                var pageContents = _pageContentRepository.GetAllList(a => a.PageId == page.Id);

                if (!pageContents.Any())
                {
                    throw new UserFriendlyException(L("PageHasNoContent"));
                }

                await CreateSectionItem(page, section, pageContents);
            }
        }

        private async Task<MenuSectionItem> CreateSectionItem(Page page, MenuSection section, List<Content> pageContents)
        {
            var menuSectionItem = MenuSectionItem.CreateMenuSectionItem(page.Name, section, page.Id);

            var id = await _menuManager.AddMenuItemAsync(menuSectionItem);

            if (menuSectionItem.Id == 0)
            {
                menuSectionItem = _menuSectionItemRepository.FirstOrDefault(id);
            }

            foreach (var pageContent in pageContents)
            {

                await CreateEditSectionItemContent(pageContent, menuSectionItem);

            }
            return menuSectionItem;
        }

        private async Task CreateEditSectionItemContent(Content pageContent, MenuSectionItem menuSectionItem)
        {
            var menuSectionItemContent =
                    MenuSectionItemContent.CreateMenuSectionItemContent(DynamicCreated, pageContent.Lang,
                        menuSectionItem);
            menuSectionItemContent.PageId = pageContent.PageId;
            await _menuManager.AddMenuItemContentAsync(menuSectionItemContent);
        }

        private async Task CreateSectionContents(IEnumerable<CategoryContent> contents, MenuSection section)
        {
            foreach (var categoryContent in contents)
            {
                var sectionContent = MenuSectionContent.CreateMenuSectionContent(categoryContent.Lang,
                DynamicCreated, section);
                await _menuManager.AddSectionContentAsync(sectionContent);
            }
        }

        private async Task<MenuSection> CreateMenuSection(Category category, Menu menu)
        {
            var section = MenuSection.CreateMenuSectionWithCategory(category.Name, menu, category);
            var sectionId = await _menuManager.AddSectionAsync(section);
            if (section.Id == 0)
            {
                section = _menuSectionRepository.FirstOrDefault(sectionId);
            }
            return section;
        }
        #endregion
    }
}



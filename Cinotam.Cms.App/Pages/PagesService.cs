using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Localization;
using Abp.Threading;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Extensions;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Events;
using Cinotam.Cms.App.Pages.Dto;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.Core.Pages;
using Cinotam.Cms.Core.Templates;
using Cinotam.Cms.DatabaseEntities.Category.Entities;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Files;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Chunk = Cinotam.Cms.App.Pages.Dto.Chunk;

namespace Cinotam.Cms.App.Pages
{
    public class PagesService : CinotamCmsAppServiceBase, IPagesService
    {
        #region Dependencies

        private readonly IPageManager _pageManager;
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Content> _contentRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<DatabaseEntities.Pages.Entities.Chunk> _chunkRepository;
        private readonly ITemplateManager _templateManager;
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IFileStoreManager _fileStoreManager;
        public IEventBus EventBus { get; set; }
        #endregion

        #region Ctor

        public PagesService(IPageManager pageManager,
            IRepository<Page> pageRepository,
            IRepository<Content> contentRepository,
            IApplicationLanguageManager applicationLanguageManager,
            ITemplateManager templateManager,
            IRepository<Category> categoryRepository,
            IRepository<DatabaseEntities.Pages.Entities.Chunk> chunkRepository, IFileStoreManager fileStoreManager)
        {
            _pageManager = pageManager;
            _pageRepository = pageRepository;
            _contentRepository = contentRepository;
            _applicationLanguageManager = applicationLanguageManager;
            _templateManager = templateManager;
            _categoryRepository = categoryRepository;
            _chunkRepository = chunkRepository;
            _fileStoreManager = fileStoreManager;
            EventBus = NullEventBus.Instance;
        }


        #endregion

        #region Implementations

        public async Task CreateEditPage(PageInput input)
        {
            await _pageManager.SaveOrEditPageAsync(new Page()
            {
                Active = false,
                Name = input.Title.Sluggify(),
                ParentPage = input.ParentId,
                TemplateName = input.TemplateName
            });

        }

        public async Task SavePageContent(PageContentInput input)
        {
            var chunks = new List<CChunk>();
            var pageContent = _contentRepository.FirstOrDefault(a => a.Page.Id == input.PageId && input.Lang == a.Lang);
            pageContent.HtmlContent = input.HtmlContent;
            var uniquePageName = pageContent.Title + pageContent.PageId + pageContent.Lang;
            var file = await _fileStoreManager.SaveFileFromBase64(uniquePageName.Sluggify(), input.Base64String.Trim(), true);
            foreach (var inputChunk in input.Chunks)
            {
                chunks.Add(new CChunk()
                {
                    PageContent = pageContent,
                    Value = inputChunk.Value,
                    Key = inputChunk.Key,
                    Order = inputChunk.Order,
                });
            }
            pageContent.PreviewImage = file.WasStoredInCloud ? file.Url : file.VirtualPath;
            await _pageManager.SavePageContentAsync(pageContent, chunks);
        }

        public async Task TogglePageStatus(int pageId)
        {
            var page = await _pageRepository.FirstOrDefaultAsync(pageId);

            page.Active = !page.Active;
            _pageRepository.Update(page);
            //if (!page.Active)
            //{
            //    _menuManager.RemoveSectionItemsForPage(pageId);
            //}
            //else
            //{
            //    if (page.IncludeInMenu)
            //    {
            //        await _menuManager.SetItemForPage(page);
            //    }
            //}
            EventBus.Trigger(new PageStateChangedData() { Page = page });
        }

        public async Task TogglePageInMenuStatus(int pageId)
        {
            var page = await _pageRepository.GetAsync(pageId);
            page.IncludeInMenu = !page.IncludeInMenu;
            _pageRepository.Update(page);
            //if (!page.IncludeInMenu)
            //{
            //    _menuManager.RemoveSectionItemsForPage(pageId);
            //}
            //else
            //{
            //    if (page.Active)
            //    {

            //        await _menuManager.SetItemForPage(page);
            //    }
            //}

            EventBus.Trigger(new PageStateChangedData() { Page = page });
        }

        public void SetPageAsMain(int pageId)
        {
            var page = _pageRepository.Get(pageId);
            page.IsMainPage = !page.IsMainPage;
            DisableOthers(pageId);
            _pageRepository.Update(page);
        }

        public async Task<string> GetMainPageSlug()
        {
            var page = await _pageRepository.FirstOrDefaultAsync(a => a.IsMainPage && a.Active);
            if (page == null) return string.Empty;
            var pageContents =
                _contentRepository.FirstOrDefault(
                    a => a.Page.Id == page.Id && a.Lang == CultureInfo.CurrentUICulture.Name);
            if (pageContents == null) return string.Empty;
            return pageContents.Url;
        }

        public Menu GetPagesMenu()
        {
            var pagesMenuList = new List<MenuDto>();
            var parentPages = _pageRepository.GetAllList(a => a.ParentPage == null && a.Active);

            foreach (var parent in parentPages)
            {
                var childs = SearchForChilds(parent);
                var content = _contentRepository.FirstOrDefault(a => a.PageId == parent.Id);
                if (content == null) continue;
                pagesMenuList.Add(new MenuDto()
                {
                    DisplayText = content.Title,
                    Childs = childs,
                    Url = content.Url
                });
            }
            return new Menu()
            {
                Menus = pagesMenuList
            };
        }

        public async Task<List<PageDto>> GetPageListDto()
        {
            var pages = await _pageRepository.GetAllListAsync();
            return pages.Select(a => new PageDto()
            {
                Id = a.Id,
                Langs = AsyncHelper.RunSync(() => GetAvailableLangs(a.Id)),
                Title = a.Name,
            }).ToList();
        }

        public async Task SetParentPage(ParentPageSetInput input)
        {
            var page = _pageRepository.FirstOrDefault(input.PageId);
            if (page == null) return;
            if (input.ParentPageId == 0)
            {
                input.ParentPageId = null;
            }
            page.ParentPage = input.ParentPageId;
            await _pageManager.SaveOrEditPageAsync(page);
        }

        public async Task<ImageAddedResult> AddImageToPage(AddImageInput input)
        {
            var specialFolder = $"/Content/PageImages/{input.PageId}/{input.Lang}";
            var fileSavedInfo = await _fileStoreManager.SaveFile(new FileManagerServiceInput()
            {
                CreateUniqueName = true,
                File = input.Image,
                VirtualFolder = specialFolder
            }, false);


            var imageInfo = _fileStoreManager.GetImageInfo(fileSavedInfo.AbsolutePath);



            return new ImageAddedResult()
            {
                Size = new[] { imageInfo.Width.ToString(), imageInfo.Height.ToString() },
                Url = fileSavedInfo.VirtualPath,
            };
        }

        public async Task<ImageAddedResult> ProcessImage(ProcessImageInput input)
        {
            var absoluteFilePath = HttpContext.Current.Server.MapPath(input.Url);

            var result =

                await Task.FromResult(_fileStoreManager.CropImage(input.Url, absoluteFilePath, input.Width, input.Crop));

            return new ImageAddedResult()
            {
                AbsolutePath = result.AbsolutePath,
                Url = result.Url
            };
        }

        public async Task<string> SaveImageFromBase64(Base64Input base64)
        {
            var path = (base64.PageId + base64.Lang).Sluggify();
            var result = await _fileStoreManager.SaveFileFromBase64(path, base64.Base64String, false, ".png");
            if (result.WasStoredInCloud)
            {
                return result.Url;
            }
            return result.VirtualPath;
        }

        public async Task<CategorySetResult> SetCategory(CategoryAssignationInput input)
        {
            if (input.CategoryId == 0)
            {
                await SetCategoryToNull(input.PageId);
                return new CategorySetResult();
            }
            var page = _pageRepository.FirstOrDefault(input.PageId);
            var oldCategoryId = page.CategoryId;
            //var exists = await CreateCategoryIfNotExists(input.Name, input.DisplayName);
            var category = await _categoryRepository.FirstOrDefaultAsync(a => a.Id == input.CategoryId);
            if (category == null) throw new UserFriendlyException(L("CategoryDontExists"));

            page.Category = category;
            await _pageManager.SaveOrEditPageAsync(page);
            if (page.CategoryId.HasValue)
            {
                EventBus.Trigger(new PageCategoryChangedData()
                {
                    PageId = page.Id,
                    CategoryId = page.CategoryId.Value,
                    OldCategoryId = oldCategoryId
                });
            }

            return new CategorySetResult();
        }

        private async Task SetCategoryToNull(int inputPageId)
        {
            var page = _pageRepository.FirstOrDefault(inputPageId);
            page.CategoryId = null;
            await _pageManager.SaveOrEditPageAsync(page);
        }

        public async Task<CategoryOutput> GetCategories()
        {
            var categories = new List<CategoryDto>();
            var allCategories = await _categoryRepository.GetAllListAsync();
            foreach (var allCategory in allCategories)
            {
                categories.Add(new CategoryDto()
                {
                    DisplayName = allCategory.DisplayName,
                    Name = allCategory.Name
                });
            }
            return new CategoryOutput()
            {
                Categories = categories
            };
        }

        public async Task<PageTitleInput> GetPageTitleForEdit(int id, string lang)
        {
            var contents = await _pageManager.GetPageContentAsync(id, lang);
            if (contents == null) return new PageTitleInput() { PageId = id, Lang = lang };
            return new PageTitleInput()
            {
                Lang = lang,
                PageId = id,
                Title = contents.Title,

            };
        }

        public async Task CreateEditPageTitle(PageTitleInput input)
        {
            var page = _pageRepository.Get(input.PageId);
            var template = await _templateManager.GetTemplateContentAsync(page.TemplateName);
            await _pageManager.SavePageContentAsync(new Content()
            {
                Title = input.Title,
                Lang = input.Lang,
                HtmlContent = template.Content,
                IsPartial = template.IsPartial,
                Url = input.Title.Sluggify(),
                PageId = page.Id,
                Page = page,

                TemplateUniqueName = page.TemplateName
            });
        }

        public async Task<PageViewOutput> GetPageViewById(int id, string lang)
        {
            var pageContent = await _pageManager.GetPageContentAsync(id, lang);
            var template = await _templateManager.GetTemplateContentAsync(pageContent.TemplateUniqueName);
            var css = template.Resources.Where(a => a.ResourceType == "css");
            var js = template.Resources.Where(a => a.ResourceType == "js");
            return new PageViewOutput()
            {
                Id = id,
                Lang = lang,
                HtmlContent = pageContent.HtmlContent,
                TemplateName = pageContent.TemplateUniqueName,
                Title = pageContent.Title,
                BreadCrums = await GetBreadCrumsForPage(id),
                IsPartial = pageContent.IsPartial,
                ShowBreadCrum = pageContent.Page.ShowBreadCrum,
                BreadCrumInContainer = pageContent.Page.BreadCrumInContainer,
                ContentId = pageContent.Id,
                JsResource = js.Select(a => new ResourceDto()
                {
                    IsCdn = a.IsCdn,
                    Url = a.ResourceUrl
                }).ToList(),
                CssResource = css.Select(a => new ResourceDto()
                {
                    IsCdn = a.IsCdn,
                    Url = a.ResourceUrl
                }).ToList()
            };
        }


        public async Task<PageViewOutput> GetPageViewBySlug(string slug)
        {
            var content = _contentRepository.FirstOrDefault(a => a.Url.ToUpper().Equals(slug.ToUpper()) && a.Page.Active);
            if (content == null) return null;
            var template = await _templateManager.GetTemplateContentAsync(content.TemplateUniqueName);
            var css = template.Resources.Where(a => a.ResourceType == "css");
            var js = template.Resources.Where(a => a.ResourceType == "js");
            if (content.Lang == CultureInfo.CurrentUICulture.Name)
            {
                return new PageViewOutput()
                {
                    HtmlContent = content.HtmlContent,
                    Id = content.PageId,
                    Lang = content.Lang,
                    TemplateName = content.TemplateUniqueName,
                    Title = content.Title,
                    IsPartial = content.IsPartial,
                    BreadCrums = await GetBreadCrumsForPage(content.PageId),
                    ShowBreadCrum = content.Page.ShowBreadCrum,
                    BreadCrumInContainer = content.Page.BreadCrumInContainer,
                    JsResource = js.Select(a => new ResourceDto()
                    {
                        IsCdn = a.IsCdn,
                        Url = a.ResourceUrl
                    }).ToList(),
                    CssResource = css.Select(a => new ResourceDto()
                    {
                        IsCdn = a.IsCdn,
                        Url = a.ResourceUrl
                    }).ToList()
                };
            }
            var currentLangContent = _contentRepository.FirstOrDefault(a => a.PageId == content.PageId && CultureInfo.CurrentUICulture.Name == a.Lang);
            if (currentLangContent == null) return null;
            return new PageViewOutput()
            {
                HtmlContent = currentLangContent.HtmlContent,
                Id = currentLangContent.PageId,
                Lang = currentLangContent.Lang,
                TemplateName = currentLangContent.TemplateUniqueName,
                Title = currentLangContent.Title,
                BreadCrums = await GetBreadCrumsForPage(content.PageId),
                IsPartial = currentLangContent.IsPartial,
            };
        }

        public Task<PageInput> GetPage(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<PageInput> GetPageForEdit(int? id)
        {
            var templates = await _templateManager.GetAvailableTemplatesAsync();
            var otherPages = _pageRepository.GetAllList();
            await _templateManager.GetTemplateContentAsync("Simple");
            if (!id.HasValue) return new PageInput()
            {
                Pages = otherPages.Select(a => new PageDto()
                {
                    Id = a.Id,
                    Title = a.Name
                }).ToList(),
                Templates = templates.Select(a => new TemplateDto()
                {
                    Name = a
                }).ToList(),
            };

            var page = await _pageRepository.GetAsync(id.Value);
            if (page == null) return new PageInput();
            return new PageInput()
            {
                Title = page.Name,
                ParentId = page.ParentPage ?? 0,
                Active = page.Active,
                Pages = otherPages.Select(a => new PageDto()
                {
                    Id = a.Id,
                    Title = a.Name
                }).ToList(),
                Templates = templates.Select(a => new TemplateDto()
                {
                    Name = a
                }).ToList(),
            };
        }

        public Task<PageDto> GetPreviewPage(int id, string name)
        {
            throw new NotImplementedException();
        }

        public ReturnModel<PageDto> GetPageList(RequestModel<object> input)
        {
            var query = _pageRepository.GetAll();
            List<Expression<Func<Page, string>>> searchs = new EditableList<Expression<Func<Page, string>>>();
            searchs.Add(a => a.Name);
            searchs.Add(a => a.Category.DisplayName);
            searchs.Add(a => a.TemplateName);
            int count;
            var pages = GenerateTableModel(input, query, searchs, "Name", out count);
            return new ReturnModel<PageDto>()
            {
                data = pages.Select(a => new PageDto()
                {
                    Id = a.Id,
                    Langs = AsyncHelper.RunSync(() => GetAvailableLangs(a.Id)),
                    Title = a.Name,
                    CategoryName = a.CategoryId.HasValue ? GetCategoryName(a) : "",
                    TemplateName = a.TemplateName,
                }).ToArray(),
                draw = input.draw,
                length = input.length,
                recordsTotal = count,
                iTotalDisplayRecords = count,
                iTotalRecords = query.Count(),
                recordsFiltered = pages.Count()
            };
        }

        public async Task<PageConfigurationObject> GetPageConfigurationObject(int id)
        {
            var page = await _pageRepository.GetAsync(id);
            var pageContents = await _contentRepository.GetAllListAsync(a => a.Page.Id == id);
            if (!pageContents.Any()) return (await CreateEmptyPageConfigurationObject(page));
            return new PageConfigurationObject()
            {
                PageName = page.Name,
                IsActive = page.Active,
                Id = id,
                ContentsByLanguage = await GetContentsByLanguage(pageContents),
                IsMainPage = page.IsMainPage,
                CategoryName = page.CategoryId.HasValue ? GetCategoryName(page) : "",
                IncludeInMenu = page.IncludeInMenu,
                TemplateName = page.TemplateName,
                ShowBreadCrum = page.ShowBreadCrum,
                BreadCrumInContainer = page.BreadCrumInContainer,
                AvailableTemplates = await _templateManager.GetTemplateContentsAsync(),
                AvailablePages = (await GetPageListDto()).Where(a => a.Id != page.Id).ToList(),
                ParentId = page.ParentPage ?? 0,
                AvailableCategoryDtos = _categoryRepository.GetAllList().Select(a => new CategoryDto()
                {
                    DisplayName = a.DisplayName,
                    Id = a.Id,
                    Name = a.Name
                }).ToList(),
                CategoryId = page.CategoryId ?? 0
            };
        }


        public async Task<List<Chunk>> GetChunks(ChunkRequest input)
        {
            var listOfChunks = new List<Chunk>();
            var chunks = await _chunkRepository.GetAllListAsync(a => a.ContentId == input.Id);
            foreach (var chunk in chunks.OrderBy(a => a.Order))
            {
                listOfChunks.Add(new Chunk()
                {
                    Key = chunk.Key,
                    Order = chunk.Order,
                    Value = chunk.Value
                });
            }
            return listOfChunks;
        }

        public async Task<PageViewOutput> GetTemplateHtml(int id, string lang, string templateName)
        {
            var html = await _templateManager.GetTemplateContentAsync(templateName);
            var pageContent = await _pageManager.GetPageContentAsync(id, lang);
            var template = await _templateManager.GetTemplateContentAsync(pageContent.TemplateUniqueName);
            var css = template.Resources.Where(a => a.ResourceType == "css");
            var js = template.Resources.Where(a => a.ResourceType == "js");
            return new PageViewOutput()
            {
                ContentId = pageContent.Id,
                Id = id,
                IsPartial = pageContent.IsPartial,
                Lang = pageContent.Lang,
                TemplateName = pageContent.TemplateUniqueName,
                Title = pageContent.Title,
                HtmlContent = html.Content,
                JsResource = js.Select(a => new ResourceDto()
                {
                    IsCdn = a.IsCdn,
                    Url = a.ResourceUrl
                }).ToList(),
                CssResource = css.Select(a => new ResourceDto()
                {
                    IsCdn = a.IsCdn,
                    Url = a.ResourceUrl
                }).ToList()
            };
        }

        private string GetCategoryName(IMayHaveCategory page)
        {
            var category = _categoryRepository.FirstOrDefault(a => a.Id == page.CategoryId);
            return category == null ? string.Empty : category.DisplayName;
        }

        #endregion

        #region Helpers

        private async Task<List<BreadCrum>> GetBreadCrumsForPage(int id)
        {
            var list = new List<BreadCrum>();

            //First we add the main page (if any is set)


            var page = _pageRepository.FirstOrDefault(a => a.Id == id);

            if (page != null)
            {
                if (!page.IsMainPage)
                {
                    var mainPage = await GetMainPage();
                    if (mainPage != null)
                    {

                        list.Add(mainPage);
                    }
                }
                await GetParentBreadCrumForPage(page, list);
                var content = await _pageManager.GetPageContentAsync(page.Id, CultureInfo.CurrentUICulture.Name);
                //This way the current page breadcrum will be added at the end of the list
                list.Add(new BreadCrum()
                {
                    DisplayName = content.Title,
                    Url = content.Url
                });
            }

            return list;

        }

        private async Task<BreadCrum> GetMainPage()
        {
            var page = await _pageRepository.FirstOrDefaultAsync(a => a.IsMainPage && a.Active);
            if (page == null) return null;
            var pageContents =
                _contentRepository.FirstOrDefault(
                    a => a.Page.Id == page.Id && a.Lang == CultureInfo.CurrentUICulture.Name);
            if (pageContents == null)
            {
                return new BreadCrum()
                {
                    DisplayName = string.Empty,
                    Url = string.Empty
                };
            }
            return new BreadCrum()
            {
                DisplayName = pageContents.Title,
                Url = pageContents.Url
            };
        }
        private List<MenuDto> SearchForChilds(Page parent)
        {
            var childs = _pageRepository.GetAllList(a => a.ParentPage == parent.Id && a.Active);
            var childList = new List<MenuDto>();
            foreach (var child in childs)
            {
                var content = _contentRepository.FirstOrDefault(a => a.PageId == child.Id);
                if (content == null) continue;
                childList.Add(new MenuDto()
                {
                    DisplayText = content.Title,
                    Url = content.Url,
                    Childs = SearchForChilds(child)
                });
            }
            return childList;
        }

        private void DisableOthers(int pageId)
        {
            var pages = _pageRepository.GetAllList(a => a.Id != pageId);
            foreach (var page in pages)
            {
                page.IsMainPage = false;
                _pageRepository.Update(page);
            }

        }
        private async Task GetParentBreadCrumForPage(Page page, List<BreadCrum> breadCrums)
        {
            if (page.ParentPage.HasValue)
            {
                var parent = _pageRepository.FirstOrDefault(a => a.Id == page.ParentPage.Value);
                await GetParentBreadCrumForPage(parent, breadCrums);
                var content = await _pageManager.GetPageContentAsync(parent.Id, CultureInfo.CurrentUICulture.Name);
                //This way the current page breadcrum will be added at the end of the list
                breadCrums.Add(new BreadCrum()
                {
                    DisplayName = content.Title,
                    Url = content.Url
                });
            }
        }
        private async Task<List<PageContentDto>> GetContentsByLanguage(List<Content> pageContents)
        {
            var allLanguages = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            var pageContentsList = new List<PageContentDto>();
            foreach (var applicationLanguage in allLanguages)
            {
                var contentWithLanguage = pageContents.FirstOrDefault(a => a.Lang == applicationLanguage.Name);
                if (contentWithLanguage == null)
                {
                    pageContentsList.Add(new PageContentDto()
                    {
                        Lang = applicationLanguage.Name,
                        HtmlContent = "",
                        LanguageIcon = applicationLanguage.Icon,
                        Slug = "",
                    });
                }
                else
                {
                    pageContentsList.Add(new PageContentDto()
                    {
                        Lang = applicationLanguage.Name,
                        LanguageIcon = applicationLanguage.Icon,
                        Slug = contentWithLanguage.Url,
                        Title = contentWithLanguage.Title,
                        TemplateUniqueName = contentWithLanguage.TemplateUniqueName,
                        Preview = contentWithLanguage.PreviewImage
                    });
                }
            }
            return pageContentsList;
        }

        public async Task SetTemplate(SetTemplateInput input)
        {
            var page = _pageRepository.Get(input.PageId);
            page.TemplateName = input.TemplateName;
            await _pageManager.SaveOrEditPageAsync(page);
        }

        public async Task DeletePage(int pageId)
        {
            var page = _pageRepository.FirstOrDefault(a => a.Id == pageId);

            await SetChildsNull(pageId);

            _pageRepository.Delete(page);

            //Remove elements from menu or other operations
            EventBus.Trigger(new PageDeletedData()
            {
                PageId = pageId
            });
        }

        private async Task SetChildsNull(int pageId)
        {
            var childPages = await _pageRepository.GetAllListAsync(a => a.ParentPage == pageId);
            foreach (var childPage in childPages)
            {
                childPage.ParentPage = null;
            }
        }

        private async Task<PageConfigurationObject> CreateEmptyPageConfigurationObject(Page page)
        {
            var allLanguages = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            var obj = new PageConfigurationObject();
            foreach (var applicationLanguage in allLanguages)
            {
                obj.ContentsByLanguage.Add(new PageContentDto()
                {
                    Lang = applicationLanguage.Name,
                    HtmlContent = "",
                    LanguageIcon = applicationLanguage.Icon,
                    Slug = "",
                });
                obj.ParentId = page.ParentPage ?? 0;
                obj.Id = page.Id;
                obj.IsMainPage = false;
                obj.CategoryName = page.CategoryId.HasValue ? GetCategoryName(page) : "";
                obj.IncludeInMenu = page.IncludeInMenu;
                obj.AvailableTemplates = await _templateManager.GetTemplateContentsAsync();
                obj.AvailablePages = (await GetPageListDto()).Where(a => a.Id != page.Id).ToList();
                obj.AvailableCategoryDtos = _categoryRepository.GetAllList().Select(a => new CategoryDto()
                {
                    DisplayName = a.DisplayName,
                    Id = a.Id,
                    Name = a.Name
                }).ToList();
            }
            return obj;
        }

        private async Task<List<Lang>> GetAvailableLangs(int pageId)
        {
            var list = new List<Lang>();
            var allContents = _contentRepository.GetAllList(a => a.Page.Id == pageId);
            foreach (var item in allContents)
            {
                var lang = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId)).FirstOrDefault(a => a.Name.Equals(item.Lang));
                if (lang != null)
                    list.Add(new Lang()
                    {
                        LangCode = lang.Name,
                        LangIcon = lang.Icon,
                    });
            }
            return list;
        }

        #endregion

    }
}


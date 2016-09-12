using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Threading;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Extensions;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Pages.Dto;
using Cinotam.Cms.Core.Pages;
using Cinotam.Cms.Core.Templates;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Pages
{
    public class PagesService : CinotamCmsAppServiceBase, IPagesService
    {
        private readonly IPageManager _pageManager;
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Content> _contentRepository;
        private readonly IRepository<Template> _templateRepository;
        private readonly ITemplateManager _templateManager;
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        public PagesService(IPageManager pageManager, IRepository<Page> pageRepository, IRepository<Content> contentRepository, IRepository<Template> templateRepository, IApplicationLanguageManager applicationLanguageManager, ITemplateManager templateManager)
        {
            _pageManager = pageManager;
            _pageRepository = pageRepository;
            _contentRepository = contentRepository;
            _templateRepository = templateRepository;
            _applicationLanguageManager = applicationLanguageManager;
            _templateManager = templateManager;
        }

        public async Task CreateEditPage(PageInput input)
        {
            await _pageManager.SaveOrEditPage(new Page()
            {
                Active = false,
                Name = input.Title,
                ParentPage = input.ParentId,
                TemplateName = input.TemplateName
            });

        }

        public async Task SavePageContent(PageContentInput input)
        {
            var pageContent = _contentRepository.FirstOrDefault(a => a.Page.Id == input.PageId && input.Lang == a.Lang);
            pageContent.HtmlContent = input.HtmlContent;
            await _pageManager.SavePageContent(pageContent);
        }

        public void TogglePageStatus(int pageId)
        {
            var page = _pageRepository.Get(pageId);
            page.Active = !page.Active;
            _pageRepository.Update(page);
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

        private void DisableOthers(int pageId)
        {
            var pages = _pageRepository.GetAllList(a => a.Id != pageId);
            foreach (var page in pages)
            {
                page.IsMainPage = false;
                _pageRepository.Update(page);
            }

        }

        public async Task<PageTitleInput> GetPageTitleForEdit(int id, string lang)
        {
            var contents = await _pageManager.GetPageContent(id, lang);
            if (contents == null) return new PageTitleInput() { PageId = id, Lang = lang };
            return new PageTitleInput()
            {
                Lang = lang,
                PageId = id,
                Title = contents.Title
            };
        }

        public async Task CreateEditPageTitle(PageTitleInput input)
        {
            var page = _pageRepository.Get(input.PageId);
            var template = await _templateManager.GetTemplateContent(page.TemplateName);
            await _pageManager.SavePageContent(new Content()
            {
                Title = input.Title,
                Lang = input.Lang,
                HtmlContent = template,
                Url = input.Title.Sluggify(),
                PageId = page.Id,
                Page = page,
                TemplateUniqueName = page.TemplateName
            });
        }

        public async Task<PageViewOutput> GetPageViewById(int id, string lang)
        {
            var pageContent = await _pageManager.GetPageContent(id, lang);
            return new PageViewOutput()
            {
                Id = id,
                Lang = lang,
                HtmlContent = pageContent.HtmlContent,
                TemplateName = pageContent.TemplateUniqueName,
                Title = pageContent.Title
            };
        }

        public PageViewOutput GetPageViewBySlug(string slug)
        {
            var content = _contentRepository.FirstOrDefault(a => a.Url.ToUpper().Equals(slug.ToUpper()) && a.Page.Active);
            if (content == null) return null;
            if (content.Lang == CultureInfo.CurrentUICulture.Name)
            {
                return new PageViewOutput()
                {
                    HtmlContent = content.HtmlContent,
                    Id = content.PageId,
                    Lang = content.Lang,
                    TemplateName = content.TemplateUniqueName,
                    Title = content.Title
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
                Title = currentLangContent.Title
            };
        }

        public Task<PageInput> GetPage(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<PageInput> GetPageForEdit(int? id)
        {
            var templates = await _templateManager.GetAvailableTemplates();
            var otherPages = _pageRepository.GetAllList();
            await _templateManager.GetTemplateContent("Simple");
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
            int count;
            var pages = GenerateTableModel(input, query, searchs, "Name", out count);
            return new ReturnModel<PageDto>()
            {
                data = pages.Select(a => new PageDto()
                {
                    Id = a.Id,
                    Langs = AsyncHelper.RunSync(() => GetAvailableLangs(a.Id)),
                    Title = a.Name,
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
            if (!pageContents.Any()) return (await CreateEmptyPageConfigurationObject(id));
            return new PageConfigurationObject()
            {
                PageName = page.Name,
                IsActive = page.Active,
                Id = id,
                ContentsByLanguage = await GetContentsByLanguage(pageContents),
                IsMainPage = page.IsMainPage

            };
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
                        Slug = ""
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
                        TemplateUniqueName = contentWithLanguage.TemplateUniqueName
                    });
                }
            }
            return pageContentsList;
        }

        private async Task<PageConfigurationObject> CreateEmptyPageConfigurationObject(int id)
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
                    Slug = ""
                });
                obj.Id = id;
                obj.IsMainPage = false;
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
                        LangIcon = lang.Icon
                    });
            }
            return list;
        }
    }
}

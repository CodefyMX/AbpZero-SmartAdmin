using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Threading;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Pages.Dto;
using Cinotam.Cms.Core.Pages;
using Cinotam.Cms.Core.Templates;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
using System;
using System.Collections.Generic;
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
                Template = GetTemplateObject(input.TemplateId),
                ParentPage = input.ParentId
            });

        }

        private Template GetTemplateObject(int? inputTemplateId)
        {
            return _templateRepository.FirstOrDefault(a => a.Id == inputTemplateId.Value);
        }

        public Task<PageInput> GetPage(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<PageInput> GetPageForEdit(int? id)
        {

            await _templateManager.GetTemplateContent("Simple");
            if (!id.HasValue) return new PageInput();
            var templates = _templateRepository.GetAllList();
            var otherPages = _pageRepository.GetAllList();
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
                    Id = a.Id,
                    Name = a.Name
                }).ToList(),
                TemplateId = GetTemplate(page.Id)
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
                    TemplateId = GetTemplate(a.Id)
                }).ToArray(),
                draw = input.draw,
                length = input.length,
                recordsTotal = count,
                iTotalDisplayRecords = count,
                iTotalRecords = query.Count(),
                recordsFiltered = pages.Count()
            };
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

        private int GetTemplate(int pageId)
        {
            var template = _templateRepository.FirstOrDefault(a => a.Page.Any(p => p.Id == pageId));
            return template?.Id ?? 0;
        }
    }
}

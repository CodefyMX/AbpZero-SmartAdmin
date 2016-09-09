using Abp.Domain.Repositories;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.DatabaseContentProvider.Provider
{
    public class DatabaseContentProvider : IDatabaseContentProvider
    {
        private readonly IRepository<Content> _contentRepository;
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Template> _templateRepository;
        public DatabaseContentProvider(IRepository<Content> contentRepository, IRepository<Page> pageRepository, IRepository<Template> templateRepository)
        {
            _contentRepository = contentRepository;
            _pageRepository = pageRepository;
            _templateRepository = templateRepository;
        }

        public bool IsFileSystemService => false;

        public async Task SaveContent(IPageContent input)
        {
            var page = _pageRepository.Get(input.PageId);
            var pageContent = new Content()
            {
                HtmlContent = input.HtmlContent,
                Lang = input.Lang,
                PageId = input.PageId,
                Page = page
            };
            await _contentRepository.InsertAndGetIdAsync(pageContent);
        }

        public async Task<IPageContent> GetPageContent(int pageId)
        {
            var content =
                await _contentRepository.FirstOrDefaultAsync(a => a.PageId == pageId && a.Lang == CultureInfo.CurrentUICulture.Name);
            return content;
        }

        public async Task<IPageContent> GetPageContent(int pageId, string language)
        {
            var page = _pageRepository.GetAllIncluding(a => a.Template).FirstOrDefault(a => a.Id == pageId);
            if (page == null) throw new InvalidOperationException(nameof(page));
            var content =
                await _contentRepository.FirstOrDefaultAsync(a => a.PageId == pageId && a.Lang == language);
            if (content != null) return content;
            var newContent = new Content()
            {
                Lang = language,
                PageId = pageId,
                HtmlContent = GetPageContentFromTemplate(page.Template.Id)
            };
            return newContent;
        }

        private string GetPageContentFromTemplate(int templateId)
        {
            return string.Empty;
        }
    }
}

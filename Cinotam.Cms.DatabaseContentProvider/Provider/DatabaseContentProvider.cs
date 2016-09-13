using Abp.Domain.Repositories;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Cinotam.Cms.DatabaseContentProvider.Provider
{
    public class DatabaseContentProvider : IDatabaseContentProvider
    {
        private readonly IRepository<Content> _contentRepository;
        private readonly IRepository<Page> _pageRepository;
        public DatabaseContentProvider(IRepository<Content> contentRepository, IRepository<Page> pageRepository)
        {
            _contentRepository = contentRepository;
            _pageRepository = pageRepository;
        }

        public bool IsFileSystemService => false;

        public async Task SaveContent(IPageContent input)
        {
            var page = _pageRepository.Get(input.PageId);
            var pageContent = _contentRepository.FirstOrDefault(a => a.Id == input.Id);
            if (pageContent != null)
            {
                pageContent.HtmlContent = input.HtmlContent.Trim();
                pageContent.Lang = input.Lang;
                pageContent.PageId = input.PageId;
                pageContent.Page = page;
                pageContent.Title = input.Title;
                pageContent.Url = input.Url;
                pageContent.TemplateUniqueName = input.TemplateUniqueName;
            }
            else
            {
                pageContent = new Content()
                {
                    HtmlContent = input.HtmlContent.Trim(),
                    Lang = input.Lang,
                    PageId = input.PageId,
                    Page = page,
                    Title = input.Title,
                    Url = input.Url,
                    TemplateUniqueName = input.TemplateUniqueName,
                };
            }

            await _contentRepository.InsertOrUpdateAndGetIdAsync(pageContent);
        }

        public async Task<IPageContent> GetPageContent(int pageId)
        {
            var content =
                await _contentRepository.FirstOrDefaultAsync(a => a.PageId == pageId && a.Lang == CultureInfo.CurrentUICulture.Name);
            return content;
        }

        public async Task<IPageContent> GetPageContent(int pageId, string language)
        {

            var page = _pageRepository.FirstOrDefault(a => a.Id == pageId);
            if (page == null) throw new InvalidOperationException(nameof(page));
            var content =
                await _contentRepository.FirstOrDefaultAsync(a => a.PageId == pageId && a.Lang == language);
            if (content != null) return content;
            var newContent = new Content()
            {
                Lang = language,
                PageId = pageId,
                HtmlContent = ""
            };
            return newContent;
        }
    }
}

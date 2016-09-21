using Abp.Domain.Repositories;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.Core.Pages.Policy;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Pages
{
    public class PageManager : IPageManager
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IPagePolicy _pagePolicy;
        public PageManager(IRepository<Page> pageRepository, IPagePolicy pagePolicy)
        {
            _pageRepository = pageRepository;
            _pagePolicy = pagePolicy;
        }

        public async Task<int> SaveOrEditPageAsync(Page page)
        {
            _pagePolicy.ValidatePage(page);
            return (await _pageRepository.InsertOrUpdateAndGetIdAsync(page));
        }

        public async Task<Page> GetPageAsync(int id)
        {
            var page = await _pageRepository.FirstOrDefaultAsync(a => a.Id == id);
            return page;
        }

        public async Task SavePageContentAsync(Content content, List<CChunk> chunks = null)
        {
            var useFileSystem = false;
            await _pagePolicy.ValidateContent(content);
            foreach (var templateContentProvider in CinotamCmsCore.PageContentProviders.Where(a => a.IsFileSystemService == useFileSystem))
            {
                await templateContentProvider.SaveContent(content);
                await templateContentProvider.AddChunks(chunks);
            }
        }

        public async Task<Content> GetPageContentAsync(int pageId, string lang = "en")
        {
            var useFileSystem = false;

            foreach (var templateContentProvider in CinotamCmsCore.PageContentProviders.Where(a => a.IsFileSystemService == useFileSystem))
            {
                var result = await templateContentProvider.GetPageContent(pageId, lang);
                if (result != null) return result as Content;
            }
            return null;
        }
    }
}

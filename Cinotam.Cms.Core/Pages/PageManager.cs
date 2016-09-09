using Abp.Domain.Repositories;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Pages
{
    public class PageManager : IPageManager
    {
        private readonly IRepository<Page> _pageRepository;

        public PageManager(IRepository<Page> pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<int> SaveOrEditPage(Page page)
        {
            return (await _pageRepository.InsertAndGetIdAsync(page));
        }

        public async Task<Page> GetPage(int id)
        {
            var page = await _pageRepository.FirstOrDefaultAsync(a => a.Id == id);
            return page;
        }

        public async Task SavePageContent(Content content)
        {
            var useFileSystem = false;

            foreach (var templateContentProvider in CinotamCmsCore.TemplateContentProviders.Where(a => a.IsFileSystemService == useFileSystem))
            {
                await templateContentProvider.SaveContent(content);
            }
        }

        public async Task<Content> GetPageContent(int pageId, string lang = "en")
        {
            var useFileSystem = false;

            foreach (var templateContentProvider in CinotamCmsCore.TemplateContentProviders.Where(a => a.IsFileSystemService == useFileSystem))
            {
                var result = await templateContentProvider.GetPageContent(pageId, lang);
                if (result != null) return result as Content;
            }
            return null;
        }
    }
}

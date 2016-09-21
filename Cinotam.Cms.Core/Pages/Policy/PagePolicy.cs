using Abp.Domain.Repositories;
using Abp.UI;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Pages.Policy
{
    public class PagePolicy : IPagePolicy
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Content> _contentRepository;
        public PagePolicy(IRepository<Page> pageRepository, IRepository<Content> contentRepository)
        {
            _pageRepository = pageRepository;
            _contentRepository = contentRepository;
        }

        public void ValidatePage(Page page)
        {
            CheckName(page);
        }

        public async Task ValidateContent(Content content)
        {
            await CheckUrlContent(content);
        }

        private async Task CheckUrlContent(Content content)
        {
            if (content.Id != 0) return;
            foreach (var templateContentProvider in CinotamCmsCore.PageContentProviders.Where(a => a.IsFileSystemService == false))
            {
                var result = await templateContentProvider.GetPageContent(content.Url);
                if (result != null) throw new UserFriendlyException("RepeatedNameForUrl");
            }
        }

        private void CheckName(Page page)
        {
            if(page.Id !=0) return;
            var isAny = _pageRepository.GetAllList(a => a.Name == page.Name).Any();
            if (isAny) throw new UserFriendlyException("RepeatedNameForPage");
        }
    }
}

using Abp.Domain.Repositories;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
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

        public Task SaveContent(IPageContent input)
        {
            throw new System.NotImplementedException();
        }

        public Task<IPageContent> GetPageContent(int pageId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IPageContent> GetPageContent(int pageId, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}

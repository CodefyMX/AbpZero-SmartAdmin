using Abp.Domain.Services;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Pages
{
    public interface IPageManager : IDomainService
    {

        Task<int> SaveOrEditPageAsync(Page page);
        Task<Page> GetPageAsync(int id);
        Task SavePageContentAsync(Content content, List<CChunk> chunks = null);
        Task<Content> GetPageContentAsync(int pageId, string lang = "en");
    }
}

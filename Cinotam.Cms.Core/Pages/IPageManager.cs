using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Pages
{
    public interface IPageManager : IDomainService
    {

        Task<int> SaveOrEditPage(Page page);
        Task<Page> GetPage(int id);
        Task SavePageContent(Content content);
        Task<Content> GetPageContent(int pageId, string lang = "en");
    }
}

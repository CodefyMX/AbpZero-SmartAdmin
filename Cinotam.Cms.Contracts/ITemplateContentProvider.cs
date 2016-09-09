using System.Threading.Tasks;

namespace Cinotam.Cms.Contracts
{
    public interface ITemplateContentProvider
    {
        Task SaveContent(IPageContentInput input);
        Task<IHtmlContentOutput> GetPageContent(int pageId);
        Task<IHtmlContentOutput> GetPageContent(int pageId, string language);
    }
}

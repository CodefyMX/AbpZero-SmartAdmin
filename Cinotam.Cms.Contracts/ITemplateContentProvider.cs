using System.Threading.Tasks;

namespace Cinotam.Cms.Contracts
{
    public interface ITemplateContentProvider
    {
        bool IsFileSystemService { get; }
        Task SaveContent(IPageContent input);
        Task<IPageContent> GetPageContent(int pageId);
        Task<IPageContent> GetPageContent(int pageId, string language);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.Contracts
{
    public interface IPageContentProvider
    {
        bool IsFileSystemService { get; }
        Task SaveContent(IPageContent input);
        Task<IPageContent> GetPageContent(int pageId);
        Task<IPageContent> GetPageContent(int pageId, string language);
        Task<IPageContent> GetPageContent(string url);
        Task AddChunks(List<CChunk> inputs);
        Task<List<IChunk>> GetChunks(int pageId, string language);
    }
}

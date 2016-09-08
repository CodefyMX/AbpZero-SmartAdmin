using System.Threading.Tasks;

namespace Cinotam.FileManager.Contracts
{
    public interface IFileManagerServiceProvider
    {
        bool IsCdnService { get; }
        Task<FileManagerServiceResult> SaveImage(IFileManagerServiceInput result);
    }
}

using System.Threading.Tasks;

namespace Cinotam.FileManager.Contracts
{
    public interface IFileManagerServiceProvider
    {
        Task<IFileManagerServiceResult> SaveImage(IFileManagerServiceInput result);
    }
}

using Abp.Domain.Services;
using Cinotam.FileManager.Contracts;

namespace Cinotam.FileManager.Local.LocalFileManager
{
    public interface ILocalFileManager : IFileManagerServiceProvider, IDomainService
    {
        string SaveFileFromBase64String(string base64String, string overrideFormat);
    }
}

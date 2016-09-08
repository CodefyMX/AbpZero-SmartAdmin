using Abp.Domain.Services;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Files.Outputs;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Files
{
    public interface IFileStoreManager : IDomainService
    {
        /// <summary>
        /// Tries to store the image in a cloud 
        /// </summary>
        /// <returns></returns>
        //SavedFileResult SaveFileToCloudService(FileSaveInput input);

        //SavedFileResult SaveFileToCloudServiceFromString(FileSaveFromStringInput input);
        //SavedFileResult SaveFileToServer(FileSaveInput input, string targetFolder);

        Task<SavedFileResult> SaveFile(IFileManagerServiceInput input, bool useCdnFirst);
    }
}

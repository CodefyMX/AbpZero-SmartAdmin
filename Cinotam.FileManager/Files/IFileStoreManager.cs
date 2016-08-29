using Abp.Domain.Services;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.FileManager.Files.Outputs;

namespace Cinotam.FileManager.Files
{
    public interface IFileStoreManager : IDomainService
    {
        /// <summary>
        /// Tries to store the image in a cloud 
        /// </summary>
        /// <returns></returns>
        SavedFileResult SaveFileToCloudService(FileSaveInput input);

        SavedFileResult SaveFileToCloudServiceFromString(FileSaveFromStringInput input);
        SavedFileResult SaveFileToServer(FileSaveInput input, string targetFolder);
    }
}

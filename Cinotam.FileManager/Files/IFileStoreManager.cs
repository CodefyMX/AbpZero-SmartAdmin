using Abp.Domain.Services;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Files.Outputs;
using System.Drawing;
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

        Task<SavedFileResult> SaveFileFromBase64(string uniquePath, string base64String, bool useCdnFirst, string overrideFormat = "");
        Image GetImageInfo(string absolutePath);
        SavedFileResult CropImage(string virtualPath, string absoluteFilePath, int inputWidth, string inputCrop);
    }
}

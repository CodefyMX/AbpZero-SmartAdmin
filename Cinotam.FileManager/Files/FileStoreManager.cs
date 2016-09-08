using Cinotam.FileManager.Cloudinary.Cloudinary;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Files.Outputs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Files
{
    public class FileStoreManager : IFileStoreManager
    {
        private readonly ICloudinaryApiConsumer _cloudinaryApiConsumer;
        public FileStoreManager(ICloudinaryApiConsumer cloudinaryApiConsumer)
        {
            _cloudinaryApiConsumer = cloudinaryApiConsumer;
        }

        #region Deprecated

        //public SavedFileResult SaveFileToCloudService(FileSaveInput input)
        //{
        //    var tempFile = SaveFileToServer(input, TempFolder);

        //    var result = _cloudinaryApiConsumer.UploadImageAndGetCdn(new SaveImageInput()
        //    {
        //        AbsoluteFileDirectory = tempFile.AbsolutePath,
        //        Folder = input.SpecialFolder,
        //        TransformationsType = input.ImageEditOptions.TransFormationType,
        //        Width = (int)input.ImageEditOptions?.Width,
        //        Height = (int)input.ImageEditOptions?.Height,
        //    });
        //    if (result.Failed)
        //    {
        //        FileSystemHelper.RemoveFile(tempFile.AbsolutePath);
        //        tempFile.WasStoredInCloud = false;
        //        return new SavedFileResult();
        //    }
        //    FileSystemHelper.RemoveFile(tempFile.AbsolutePath);
        //    return new SavedFileResult()
        //    {
        //        SecureUrl = result.SecureUrl,
        //        Url = result.Url,
        //        WasStoredInCloud = true,
        //        FileName = result.PublicId
        //    };
        //}

        //public SavedFileResult SaveFileToCloudServiceFromString(FileSaveFromStringInput input)
        //{
        //    switch (input.FileType)
        //    {
        //        case ValidFileTypes.Image:

        //            var result = _cloudinaryApiConsumer.UploadImageAndGetCdn(new SaveImageInput()
        //            {
        //                AbsoluteFileDirectory = input.File,
        //                Folder = input.SpecialFolder,
        //                TransformationsType = input.ImageEditOptions.TransFormationType,
        //                Width = (int)input.ImageEditOptions?.Width,
        //                Height = (int)input.ImageEditOptions?.Height,
        //            });
        //            if (result.Failed)
        //            {
        //                return new SavedFileResult();
        //            }
        //            return new SavedFileResult()
        //            {
        //                SecureUrl = result.SecureUrl,
        //                Url = result.Url,
        //                WasStoredInCloud = true,
        //                FileName = result.PublicId
        //            };
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        //public SavedFileResult SaveFileToServer(FileSaveInput input, string virtualFolder)
        //{
        //    var fileExtension = FileSystemHelper.GetExtension(input.File.FileName);
        //    var fileName = input.File.FileName;
        //    if (input.CreateUniqueName)
        //    {
        //        fileName = Guid.NewGuid().ToString().Truncate(8) + fileExtension;
        //    }
        //    FileSystemHelper.CreateFolder(virtualFolder);
        //    var absolutePath = FileSystemHelper.GetAbsolutePath(virtualFolder);
        //    var route = absolutePath + fileName;
        //    var virtualFullRoute = virtualFolder + fileName;
        //    input.File.SaveAs(route);
        //    return new SavedFileResult()
        //    {
        //        AbsolutePath = route,
        //        FileName = fileName,
        //        VirtualPath = virtualFullRoute,
        //        WasStoredInCloud = false
        //    };
        //}


        #endregion

        public async Task<SavedFileResult> SaveFile(IFileManagerServiceInput input, bool cdnServicesFirst)
        {
            var providers = FileManagerModule.FileManagerServiceProviders.OrderBy(a => a.IsCdnService).ToList();
            if (cdnServicesFirst)
            {
                providers = providers.OrderByDescending(a => a.IsCdnService).ToList();
            }
            foreach (var fileManagerServiceProvider in providers)
            {
                var result = await fileManagerServiceProvider.SaveImage(input);
                if (result.ImageSavedInCdn)
                {
                    return new SavedFileResult()
                    {
                        AbsolutePath = string.Empty,
                        FileName = result.FileName,
                        Url = result.CdnUrl,
                        VirtualPath = string.Empty,
                        WasStoredInCloud = true
                    };
                }
                if (result.ImageSavedInServer)
                {
                    return new SavedFileResult()
                    {
                        AbsolutePath = result.LocalUrl,
                        FileName = result.FileName,
                        SecureUrl = string.Empty,
                        Url = string.Empty,
                        VirtualPath = result.VirtualPathResult,
                        WasStoredInCloud = false
                    };
                }
            }
            throw new InvalidOperationException(nameof(IFileManagerServiceProvider));
        }
    }
}

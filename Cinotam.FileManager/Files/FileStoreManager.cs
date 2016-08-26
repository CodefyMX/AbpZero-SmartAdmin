using Abp.Extensions;
using Cinotam.FileManager.Cloudinary.Cloudinary;
using Cinotam.FileManager.Cloudinary.Cloudinary.Inputs;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.FileManager.Files.Outputs;
using Cinotam.FileManager.FileSystemHelpers;
using Cinotam.FileManager.FileTypes;
using System;

namespace Cinotam.FileManager.Files
{
    public class FileStoreManager : IFileStoreManager
    {
        private readonly ICloudinaryApiConsumer _cloudinaryApiConsumer;
        private const string TempFolder = "/Content/Temp/";
        public FileStoreManager(ICloudinaryApiConsumer cloudinaryApiConsumer)
        {
            _cloudinaryApiConsumer = cloudinaryApiConsumer;
        }
        public SavedFileResult SaveFileToCloudService(FileSaveInput input)
        {
            var tempFile = SaveFileToServer(input, TempFolder);

            switch (input.FileType)
            {
                case ValidFileTypes.Image:

                    var result = _cloudinaryApiConsumer.UploadImageAndGetCdn(new SaveImageInput()
                    {
                        AbsoluteFileDirectory = tempFile.AbsolutePath,
                        Folder = input.SpecialFolder,
                        TransformationsType = input.ImageEditOptions.TransFormationType,
                        Width = (int)input.ImageEditOptions?.Width,
                        Height = (int)input.ImageEditOptions?.Height,
                    });
                    if (result.Failed)
                    {
                        FileSystemHelper.RemoveFile(tempFile.AbsolutePath);
                        tempFile.WasStoredInCloud = false;
                        return new SavedFileResult();
                    }
                    return new SavedFileResult()
                    {
                        SecureUrl = result.SecureUrl,
                        Url = result.Url,
                        WasStoredInCloud = true,
                        FileName = result.PublicId
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public SavedFileResult SaveFileToServer(FileSaveInput input, string virtualFolder)
        {
            var fileExtension = FileSystemHelper.GetExtension(input.File.FileName);
            var fileName = input.File.FileName;
            if (input.CreateUniqueName)
            {
                fileName = Guid.NewGuid().ToString().Truncate(8) + fileExtension;
            }
            FileSystemHelper.CreateFolder(virtualFolder);
            var absolutePath = FileSystemHelper.GetAbsolutePath(virtualFolder);
            var route = absolutePath + fileName;
            var virtualFullRoute = virtualFolder + fileName;
            input.File.SaveAs(route);
            return new SavedFileResult()
            {
                AbsolutePath = route,
                FileName = fileName,
                VirtualPath = virtualFullRoute,
                WasStoredInCloud = false
            };
        }
    }
}

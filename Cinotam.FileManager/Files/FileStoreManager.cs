using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.FileManager.Files.Outputs;
using Cinotam.FileManager.Local.LocalFileManager;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Files
{
    public class FileStoreManager : IFileStoreManager
    {
        private readonly ILocalFileManager _localFileManager;

        public FileStoreManager(ILocalFileManager localFileManager)
        {
            _localFileManager = localFileManager;
        }
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

        public async Task<SavedFileResult> SaveFileFromBase64(string uniquePath, string base64String, bool useCdnFirst, string overrideFormat = "")
        {
            var absolutePath = _localFileManager.SaveFileFromBase64String(base64String, overrideFormat);
            var fileSaveFromStringInput = new FileSaveFromStringInput()
            {
                CreateUniqueName = false,
                FilePath = absolutePath,
                SpecialFolder = uniquePath,
                OverrideFormat = overrideFormat,
                VirtualFolder = "/Content/Images/",
                Properties =
                {
                    ["TransformationType"] = 0
                },
            };
            var providers = FileManagerModule.FileManagerServiceProviders.OrderBy(a => a.IsCdnService).ToList();
            if (useCdnFirst)
            {
                providers = providers.OrderByDescending(a => a.IsCdnService).ToList();
            }
            foreach (var fileManagerServiceProvider in providers)
            {
                var result = await fileManagerServiceProvider.SaveImage(fileSaveFromStringInput);
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

        public Image GetImageInfo(string absolutePath)
        {
            return Image.FromFile(absolutePath);
        }

        public SavedFileResult CropImage(string virtualPath, string absoluteFilePath, int inputWidth, string inputCrop)
        {
            var cropCoords = CreateCropCoordsFromString(inputCrop);
            var image = GetImageInfo(absoluteFilePath);

            var gr = Graphics.FromImage(image);




            return new SavedFileResult()
            {
                VirtualPath = virtualPath,
                AbsolutePath = absoluteFilePath,
            };
        }


        private int Y = 0;
        private int X = 1;
        private int Yi = 2;
        private int Xi = 3;
        private CropCoords CreateCropCoordsFromString(string input)
        {
            var splitInfo = input.Split(',');
            var cropCoords = new CropCoords();
            for (int s = 0; s < splitInfo.Length; s++)
            {
                if (s == X)
                {
                    cropCoords.X = Convert.ToDouble(splitInfo[s]);
                }
                if (s == Y)
                {
                    cropCoords.Y = Convert.ToDouble(splitInfo[s]);
                }
                if (s == Xi)
                {
                    cropCoords.Xi = Convert.ToDouble(splitInfo[s]);
                }
                if (s == Yi)
                {
                    cropCoords.Yi = Convert.ToDouble(splitInfo[s]);
                }
            }
            return cropCoords;
        }
    }
}

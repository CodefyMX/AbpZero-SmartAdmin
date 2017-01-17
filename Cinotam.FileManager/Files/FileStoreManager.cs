using Abp.Dependency;
using Castle.Core.Internal;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.FileManager.Files.Outputs;
using Cinotam.FileManager.Local.LocalFileManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Files
{
    public class FileStoreManager : IFileStoreManager
    {
        private readonly ILocalFileManager _localFileManager;
        private readonly IIocManager _iocManager;
        public FileStoreManager(ILocalFileManager localFileManager)
        {
            _localFileManager = localFileManager;
            _iocManager = IocManager.Instance;
        }
        public async Task<SavedFileResult> SaveFile(IFileManagerServiceInput input, bool cdnServicesFirst)
        {

            var providers = GetProviders();

            if (cdnServicesFirst)
            {
                providers = providers.OrderByDescending(a => a.IsCdnService).ToList();
            }
            foreach (var fileManagerServiceProvider in providers)
            {
                var result = await fileManagerServiceProvider.SaveImage(input);
                if (result.ImageSavedInCdn)
                {
                    ReleaseAll(providers);
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
                    ReleaseAll(providers);
                    return new SavedFileResult()
                    {
                        AbsolutePath = result.LocalUrl,
                        FileName = result.FileName,
                        SecureUrl = result.VirtualPathResult.IsNullOrEmpty() ? result.CdnUrl : result.VirtualPathResult,
                        Url = result.VirtualPathResult.IsNullOrEmpty() ? result.CdnUrl : result.VirtualPathResult,
                        VirtualPath = result.VirtualPathResult,
                        WasStoredInCloud = false
                    };
                }
            }
            ReleaseAll(providers);
            throw new InvalidOperationException(nameof(IFileManagerServiceProvider));
        }

        private void ReleaseAll(List<IFileManagerServiceProvider> providers)
        {
            foreach (var fileManagerServiceProvider in providers)
            {
                Release(fileManagerServiceProvider);
            }
        }

        private List<IFileManagerServiceProvider> GetProviders()
        {
            var services = new List<IFileManagerServiceProvider>();
            foreach (var fileManagerServiceProvider in FileManagerModule.FileManagerServiceProviders)
            {
                var resolved = _iocManager.Resolve(fileManagerServiceProvider);
                services.Add((IFileManagerServiceProvider)resolved);
            }
            return services;
        }

        private void Release(IFileManagerServiceProvider service)
        {
            _iocManager.Release(service);
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
            var providers = GetProviders();
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
    }
}

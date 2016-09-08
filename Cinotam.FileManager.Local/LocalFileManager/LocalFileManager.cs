using Abp.Extensions;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Contracts.FileSystemHelpers;
using System;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Local.LocalFileManager
{
    public class LocalFileManager : ILocalFileManager
    {
        public bool IsCdnService => false;
        private const string LocalUserImagesFolder = "/Content/Users/ProfilePictures/{0}/";
        public async Task<FileManagerServiceResult> SaveImage(IFileManagerServiceInput input)
        {
            var fileExtension = FileSystemHelper.GetExtension(input.File.FileName);
            var fileName = input.File.FileName;
            string route;
            string absolutePath;
            if (input.CreateUniqueName)
            {
                fileName = Guid.NewGuid().ToString().Truncate(8) + fileExtension;
            }

            if (string.IsNullOrEmpty(input.VirtualFolder))
            {
                input.VirtualFolder = string.Format(LocalUserImagesFolder, input.SpecialFolder);
                absolutePath = FileSystemHelper.GetAbsolutePath(input.VirtualFolder);
                route = absolutePath + fileName;
            }
            else
            {
                absolutePath = FileSystemHelper.GetAbsolutePath(input.VirtualFolder);
                route = absolutePath + input.VirtualFolder + fileName;
            }
            FileSystemHelper.CreateFolder(input.VirtualFolder);
            var virtualFullRoute = input.VirtualFolder + fileName;
            input.File.SaveAs(route);
            await Task.FromResult(0);
            return new FileManagerServiceResult()
            {
                LocalUrl = route,
                FileName = fileName,
                VirtualPathResult = virtualFullRoute,
                ImageSaved = true,
                ImageSavedInCdn = false,
                ImageSavedInServer = true
            };
        }
    }
}

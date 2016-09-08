using Abp.Extensions;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Contracts.FileSystemHelpers;
using Cinotam.FileManager.Local.LocalFileManager.Output;
using System;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Local.LocalFileManager
{
    public class LocalFileManager : ILocalFileManager
    {
        public async Task<IFileManagerServiceResult> SaveImage(IFileManagerServiceInput input)
        {
            var fileExtension = FileSystemHelper.GetExtension(input.File.FileName);
            var fileName = input.File.FileName;
            if (input.CreateUniqueName)
            {
                fileName = Guid.NewGuid().ToString().Truncate(8) + fileExtension;
            }
            FileSystemHelper.CreateFolder(input.VirtualFolder);
            var absolutePath = FileSystemHelper.GetAbsolutePath(input.VirtualFolder);
            var route = absolutePath + fileName;
            var virtualFullRoute = input.VirtualFolder + fileName;
            input.File.SaveAs(route);
            await Task.FromResult(0);
            return new LocalSaveResult()
            {
                LocalUrl = route,
                FileName = fileName,
                VirtualPath = virtualFullRoute,
                ImageSavedInServer = true

            };
        }
    }
}

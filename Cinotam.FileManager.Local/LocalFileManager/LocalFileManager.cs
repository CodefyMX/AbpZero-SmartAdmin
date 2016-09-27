using Abp.Extensions;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Contracts.FileSystemHelpers;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Cinotam.FileManager.Local.LocalFileManager
{
    public class LocalFileManager : ILocalFileManager
    {
        public bool IsCdnService => false;
        private const string LocalTempImagesFolder = "/Content/Temp/";
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
        /// <summary>
        /// Returns the absolute path where the image was saved
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public string SaveFileFromBase64String(string base64String)
        {
            var image = ConvertToImage(base64String);

            var fileName = Guid.NewGuid();

            var absolutePath = HttpContext.Current.Server.MapPath(LocalTempImagesFolder);

            var absolutePathWithFileName = absolutePath + fileName;

            FileSystemHelper.CreateFolder(LocalTempImagesFolder);

            image.Save(absolutePathWithFileName);

            return absolutePathWithFileName;
        }

        private Image ConvertToImage(string base64String)
        {
            base64String = SanitizeBase64String(base64String);
            var imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                var image = Image.FromStream(ms, true);
                return image;
            }
        }

        private string SanitizeBase64String(string source)
        {
            var base64 = source.Substring(source.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            return base64;
        }
    }
}

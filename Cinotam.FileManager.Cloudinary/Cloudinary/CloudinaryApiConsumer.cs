
using Abp.Auditing;
using Abp.Extensions;
using Cinotam.FileManager.Cloudinary.Cloudinary.Results;
using Cinotam.FileManager.Cloudinary.Credentials;
using Cinotam.FileManager.Cloudinary.Credentials.Helpers;
using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Contracts.FileSystemHelpers;
using CloudinaryDotNet.Actions;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Cinotam.FileManager.Cloudinary.Cloudinary
{
    [Audited]
    public class CloudinaryApiConsumer : ICloudinaryApiConsumer
    {
        private readonly CloudinaryDotNet.Cloudinary _instance;
        private const string TempFolder = "/App_Data/Temp/";
        public CloudinaryApiConsumer(ICredentials credentials)
        {
            _instance = credentials.GetCloudinaryInstance(new CloudinaryIdentityObject()
            {
                ApiKey = "579477425489276",
                CloudName = "cinotamtest",
                ApiSecret = "3PmIOQESQKUjU4gg5KJW_q3z2vc"
            });
        }

        #region Deprecated

        //public virtual CloudinaryImageUploadResult UploadImageAndGetCdn(SaveImageInput input)
        //{
        //    try
        //    {
        //        var uploadParams = new ImageUploadParams()
        //        {
        //            File = new FileDescription(input.AbsoluteFileDirectory),
        //            Folder = string.IsNullOrEmpty(input.Folder) ? null : input.Folder,
        //            Transformation = Transformations.TransFormationsConfig.GetTransformationConfiguration(input)
        //        };
        //        var result = _instance.Upload(uploadParams);
        //        return new CloudinaryImageUploadResult()
        //        {
        //            SecureUrl = result.SecureUri.AbsoluteUri,
        //            Url = result.Uri.AbsoluteUri,
        //            PublicId = result.PublicId,
        //            Failed = false
        //        };
        //    }
        //    catch (System.Exception)
        //    {
        //        return new CloudinaryImageUploadResult()
        //        {
        //            Failed = true
        //        };
        //    }

        //}

        #endregion

        public bool IsCdnService => true;

        public async Task<FileManagerServiceResult> SaveImage(IFileManagerServiceInput input)
        {
            try
            {
                var filePath = string.IsNullOrEmpty(input.FilePath) ? SaveFileInTempFolder(input.File, input.CreateUniqueName) : input.FilePath;
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath),
                    Folder = string.IsNullOrEmpty(input.SpecialFolder) ? null : input.SpecialFolder,
                    Transformation = Transformations.TransFormationsConfig.GetTransformationConfiguration(input)
                };
                await Task.FromResult(0);
                var result = _instance.Upload(uploadParams);
                return new CloudinaryImageUploadResult()
                {
                    SecureUrl = result.SecureUri.AbsoluteUri,
                    Url = result.Uri.AbsoluteUri,
                    PublicId = result.PublicId,
                    CdnUrl = result.Uri.AbsoluteUri,
                    FileName = result.PublicId,
                    ImageSaved = true,
                    ImageSavedInCdn = true
                };
            }
            catch (Exception)
            {
                return new CloudinaryImageUploadResult()
                {
                    ImageSaved = false
                };
            }
        }

        private string SaveFileInTempFolder(HttpPostedFileBase inputFile, bool uniqueName)
        {
            var fileExtension = FileSystemHelper.GetExtension(inputFile.FileName);
            var fileName = inputFile.FileName;
            if (uniqueName)
            {
                fileName = Guid.NewGuid().ToString().Truncate(8) + fileExtension;
            }
            FileSystemHelper.CreateFolder(TempFolder);
            var absolutePath = FileSystemHelper.GetAbsolutePath(TempFolder);
            var route = absolutePath + fileName;
            inputFile.SaveAs(route);
            return route;
        }
    }
}

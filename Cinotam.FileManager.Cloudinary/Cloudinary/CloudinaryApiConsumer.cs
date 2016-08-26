using Cinotam.FileManager.Cloudinary.Cloudinary.Results;
using Cinotam.FileManager.Cloudinary.Credentials;
using Cinotam.FileManager.Cloudinary.Credentials.Helpers;
using CloudinaryDotNet.Actions;

namespace Cinotam.FileManager.Cloudinary.Cloudinary
{
    public class CloudinaryApiConsumer : ICloudinaryApiConsumer
    {
        private readonly CloudinaryDotNet.Cloudinary _instance;
        public CloudinaryApiConsumer(ICredentials credentials)
        {
            _instance = credentials.GetCloudinaryInstance(new CloudinaryIdentityObject()
            {
                ApiKey = "579477425489276",
                CloudName = "cinotamtest",
                ApiSecret = "3PmIOQESQKUjU4gg5KJW_q3z2vc"
            });
        }

        public virtual CloudinaryImageUploadResult UploadImageAndGetCdn(string absoluteFileDirectory)
        {
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(absoluteFileDirectory)
                };
                var result = _instance.Upload(uploadParams);
                return new CloudinaryImageUploadResult()
                {
                    SecureUrl = result.SecureUri.AbsoluteUri,
                    Url = result.Uri.AbsoluteUri,
                    PublicId = result.PublicId,
                    Failed = false
                };
            }
            catch (System.Exception)
            {
                return new CloudinaryImageUploadResult()
                {
                    Failed = true
                };
            }

        }
    }
}

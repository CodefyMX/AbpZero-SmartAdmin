using Abp.Auditing;
using Cinotam.FileManager.Cloudinary.Credentials.Helpers;
using CloudinaryDotNet;

namespace Cinotam.FileManager.Cloudinary.Credentials
{
    public class Credentials : ICredentials
    {
        [DisableAuditing]
        public virtual CloudinaryDotNet.Cloudinary GetCloudinaryInstance(CloudinaryIdentityObject credentials)
        {
            var account = new Account()
            {
                ApiKey = credentials.ApiKey,
                ApiSecret = credentials.ApiSecret,
                Cloud = credentials.CloudName
            };

            var instance = new CloudinaryDotNet.Cloudinary(account);
            return instance;
        }
    }
}

using Abp.Dependency;
using Cinotam.FileManager.Cloudinary.Credentials.Helpers;

namespace Cinotam.FileManager.Cloudinary.Credentials
{
    public interface ICredentials : ISingletonDependency
    {
        CloudinaryDotNet.Cloudinary GetCloudinaryInstance(CloudinaryIdentityObject credentials);
    }
}

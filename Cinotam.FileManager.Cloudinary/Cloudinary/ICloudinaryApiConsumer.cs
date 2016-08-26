using Abp.Domain.Services;
using Cinotam.FileManager.Cloudinary.Cloudinary.Results;

namespace Cinotam.FileManager.Cloudinary.Cloudinary
{
    public interface ICloudinaryApiConsumer : IDomainService
    {
        CloudinaryImageUploadResult UploadImageAndGetCdn(string absoluteFileDirectory);
    }
}

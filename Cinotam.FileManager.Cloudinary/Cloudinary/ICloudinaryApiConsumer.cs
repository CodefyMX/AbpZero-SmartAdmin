using Abp.Domain.Services;
using Cinotam.FileManager.Contracts;

namespace Cinotam.FileManager.Cloudinary.Cloudinary
{
    public interface ICloudinaryApiConsumer : IDomainService, IFileManagerServiceProvider
    {
        //CloudinaryImageUploadResult UploadImageAndGetCdn(SaveImageInput input);
    }
}

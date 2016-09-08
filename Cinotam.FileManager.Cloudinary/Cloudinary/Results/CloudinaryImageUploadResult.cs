using Cinotam.FileManager.Contracts;

namespace Cinotam.FileManager.Cloudinary.Cloudinary.Results
{
    //For more properties check http://cloudinary.com/documentation/dotnet_integration#getting_started_guide
    public class CloudinaryImageUploadResult : FileManagerServiceResult
    {
        public string SecureUrl { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}

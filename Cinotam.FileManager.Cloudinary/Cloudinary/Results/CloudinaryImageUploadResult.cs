namespace Cinotam.FileManager.Cloudinary.Cloudinary.Results
{
    //For more properties check http://cloudinary.com/documentation/dotnet_integration#getting_started_guide
    public class CloudinaryImageUploadResult
    {
        public string SecureUrl { get; set; }
        public string Url { get; set; }
        public bool Failed { get; set; }
        public string PublicId { get; set; }
    }
}

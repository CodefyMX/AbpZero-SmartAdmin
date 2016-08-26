using Cinotam.FileManager.SharedTypes.Enums;

namespace Cinotam.FileManager.Cloudinary.Cloudinary.Inputs
{
    public class SaveImageInput
    {
        public string AbsoluteFileDirectory { get; set; }
        public string Folder { get; set; }
        public TransformationsTypes TransformationsType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

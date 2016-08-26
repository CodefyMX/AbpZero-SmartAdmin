using Cinotam.FileManager.FileTypes;
using Cinotam.FileManager.SharedTypes.Enums;
using System.Web;

namespace Cinotam.FileManager.Files.Inputs
{
    public class FileSaveInput
    {

        public ValidFileTypes FileType { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string SpecialFolder { get; set; }

        /// <summary>
        /// Has no effect if the file is going to be stored in the cloud
        /// </summary>
        public bool CreateUniqueName { get; set; }
        public ImageEditOptionsRequest ImageEditOptions { get; set; }
    }

    public class ImageEditOptionsRequest
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public TransformationsTypes TransFormationType { get; set; }
    }
}

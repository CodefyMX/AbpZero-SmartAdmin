using Cinotam.FileManager.FileTypes;
using System.Web;

namespace Cinotam.FileManager.Files.Inputs
{
    public class FileSaveInput
    {
        public ValidFileTypes FileType { get; set; }
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// Has no effect if the file is going to be stored in the cloud
        /// </summary>
        public bool CreateUniqueName { get; set; }
    }
}

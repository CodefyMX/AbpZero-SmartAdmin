using Cinotam.FileManager.FileTypes;

namespace Cinotam.FileManager.Files.Inputs
{
    public class FileSaveFromStringInput
    {
        public ValidFileTypes FileType { get; set; }
        public string File { get; set; }
        public string SpecialFolder { get; set; }

        /// <summary>
        /// Has no effect if the file is going to be stored in the cloud
        /// </summary>
        public bool CreateUniqueName { get; set; }
        public ImageEditOptionsRequest ImageEditOptions { get; set; }
    }
}

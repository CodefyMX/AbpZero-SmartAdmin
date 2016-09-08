using Cinotam.FileManager.Contracts;

namespace Cinotam.FileManager.Local.LocalFileManager.Output
{
    public class LocalSaveResult : IFileManagerServiceResult
    {
        public LocalSaveResult()
        {
            ImageSavedInCdn = false;
            ImageSavedInServer = false;
        }
        public bool ImageSavedInCdn { get; }
        public bool ImageSavedInServer { get; set; }
        public string ServiceName { get; set; }
        public string CdnUrl { get; set; }
        public string LocalUrl { get; set; }
        public string VirtualPath { get; set; }
        public string FileName { get; set; }
    }
}

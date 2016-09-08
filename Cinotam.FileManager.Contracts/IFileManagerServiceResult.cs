namespace Cinotam.FileManager.Contracts
{
    public interface IFileManagerServiceResult
    {
        bool ImageSavedInCdn { get; }
        bool ImageSavedInServer { get; }
        string ServiceName { get; set; }
        string CdnUrl { get; set; }
        string LocalUrl { get; set; }
        string FileName { get; set; }
        string VirtualPathResult { get; set; }
        bool ImageSaved { get; set; }
    }
}

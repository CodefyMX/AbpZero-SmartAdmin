namespace Cinotam.FileManager.Contracts
{
    /// <summary>
    /// Inspiration from
    /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/763e718341a42ef1799d9ded5fae228f652d7a6b/src/Abp/Notifications/NotificationData.cs
    /// </summary>
    public class FileManagerServiceResult : IFileManagerServiceResult
    {
        public FileManagerServiceResult()
        {
            ImageSavedInCdn = false;
            ImageSavedInServer = false;

        }
        public bool ImageSavedInCdn { get; set; }
        public bool ImageSavedInServer { get; set; }
        public string ServiceName { get; set; }
        public string CdnUrl { get; set; }
        public string LocalUrl { get; set; }
        public string FileName { get; set; }
        public string VirtualPathResult { get; set; }
        public bool ImageSaved { get; set; }
    }
}

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
        public bool ImageSavedInCdn { get; }
        public bool ImageSavedInServer { get; }
        public string ServiceName { get; set; }
        public string CdnUrl { get; set; }
        public string LocalUrl { get; set; }

    }
}

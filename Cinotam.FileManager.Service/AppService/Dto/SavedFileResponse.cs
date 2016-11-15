namespace Cinotam.FileManager.Service.AppService.Dto
{
    public class SavedFileResponse
    {
        public string Url { get; set; }
        public string FileName { get; set; }
        public bool StoredInCloud { get; set; }
    }
}
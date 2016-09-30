namespace Cinotam.FileManager.Files.Outputs
{
    public class SavedFileResult
    {
        public string AbsolutePath { get; set; }
        public string VirtualPath { get; set; }

        public string SecureUrl { get; set; }
        public string Url { get; set; }
        public bool WasStoredInCloud { get; set; }
        public string FileName { get; set; }
        public string Dimensions { get; set; }
    }
}

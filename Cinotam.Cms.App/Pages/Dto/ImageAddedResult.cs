namespace Cinotam.Cms.App.Pages.Dto
{
    public class ImageAddedResult
    {
        public string Url { get; set; }
        public string[] Size { get; set; }
        public string AbsolutePath { get; set; }
    }

    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

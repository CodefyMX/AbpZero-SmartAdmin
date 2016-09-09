namespace Cinotam.Cms.Contracts
{
    public interface IPageContent
    {
        string HtmlContent { get; set; }
        string Lang { get; set; }
        int PageId { get; set; }
    }
}

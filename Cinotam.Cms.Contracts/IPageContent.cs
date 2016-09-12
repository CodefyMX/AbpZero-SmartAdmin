namespace Cinotam.Cms.Contracts
{
    public interface IPageContent
    {
        string HtmlContent { get; set; }
        string Lang { get; set; }
        int PageId { get; set; }
        string Title { get; set; }
        string Url { get; set; }
        string TemplateUniqueName { get; set; }
        int Id { get; set; }
    }
}

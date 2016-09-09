namespace Cinotam.Cms.Contracts
{
    public interface IPageContentInput
    {
        string Content { get; set; }
        string Lang { get; set; }
        int PageId { get; set; }
    }
}

namespace Cinotam.Cms.Contracts
{
    public interface IChunk
    {
        string Key { get; set; }
        string Value { get; set; }
        IPageContent PageContent { get; set; }
    }
}

namespace Cinotam.Cms.Contracts
{
    public class CChunk : IChunk
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public IPageContent PageContent { get; set; }
        public int Order { get; set; }
    }
}

namespace Cinotam.Cms.Contracts
{
    public class CResource : IResource
    {
        public string ResourceType { get; set; }
        public string ResourceUrl { get; set; }
        public ITemplateContent Template { get; set; }
        public string Description { get; set; }
        public bool IsCdn { get; set; }
    }
}

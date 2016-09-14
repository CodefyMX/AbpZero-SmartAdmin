namespace Cinotam.Cms.Contracts
{
    public interface IResource
    {
        string ResourceType { get; set; }
        string ResourceUrl { get; set; }
        ITemplateContent Template { get; set; }
    }
}

using Abp.Domain.Entities;

namespace Cinotam.Cms.Contracts
{
    public interface IResource : IMayHaveTenant
    {
        string ResourceType { get; set; }
        string ResourceUrl { get; set; }
        ITemplateContent Template { get; set; }
        string Description { get; set; }
        bool IsCdn { get; set; }
    }

}

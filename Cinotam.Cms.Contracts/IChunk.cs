using Abp.Domain.Entities;

namespace Cinotam.Cms.Contracts
{
    public interface IChunk : IMayHaveTenant
    {
        string Key { get; set; }
        string Value { get; set; }
        IPageContent PageContent { get; set; }
        int Order { get; set; }
    }
}

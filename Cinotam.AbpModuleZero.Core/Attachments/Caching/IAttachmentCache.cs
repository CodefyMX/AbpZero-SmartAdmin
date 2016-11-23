using Abp.Domain.Entities.Caching;

namespace Cinotam.AbpModuleZero.Attachments.Caching
{
    public interface IAttachmentCache : IEntityCache<AttachmentCacheItem>
    {
    }
}

using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Cinotam.AbpModuleZero.Attachments.Entities;

namespace Cinotam.AbpModuleZero.Attachments.Caching
{
    public class AttachmentCache : EntityCache<Attachment, AttachmentCacheItem>, IAttachmentCache, ITransientDependency
    {
        public AttachmentCache(ICacheManager cacheManager, IRepository<Attachment, int> repository, string cacheName = AbpModuleZeroConsts.AttachmentsCacheName) : base(cacheManager, repository, cacheName)
        {
        }




    }
}

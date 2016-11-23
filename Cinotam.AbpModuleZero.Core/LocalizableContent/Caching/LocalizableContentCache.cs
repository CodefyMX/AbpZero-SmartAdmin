using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cinotam.AbpModuleZero.LocalizableContent.Caching
{
    public class LocalizableContentCache : EntityCache<AbpCinotamLocalizableContent, AbpCinotamLocalizableContentCacheItem>, ILocalizableContentCache, ITransientDependency
    {
        public LocalizableContentCache(ICacheManager cacheManager, IRepository<AbpCinotamLocalizableContent, int> repository, string cacheName = AbpModuleZeroConsts.LocalizableContentCacheName) : base(cacheManager, repository, cacheName)
        {
        }



        public IEnumerable<AbpCinotamLocalizableContentCacheItem> GetElements(Expression<Func<AbpCinotamLocalizableContent, bool>> expression)
        {
            var cache = CacheManager.GetCache(AbpModuleZeroConsts.LocalizableContentCacheName);
            return new List<AbpCinotamLocalizableContentCacheItem>();
        }

    }
}

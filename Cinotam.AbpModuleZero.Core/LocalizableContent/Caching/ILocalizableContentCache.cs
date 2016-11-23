using Abp.Domain.Entities.Caching;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cinotam.AbpModuleZero.LocalizableContent.Caching
{
    public interface ILocalizableContentCache : IEntityCache<AbpCinotamLocalizableContentCacheItem>
    {
        /// <summary>
        /// Not working for now
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<AbpCinotamLocalizableContentCacheItem> GetElements(Expression<Func<AbpCinotamLocalizableContent, bool>> expression);
    }
}

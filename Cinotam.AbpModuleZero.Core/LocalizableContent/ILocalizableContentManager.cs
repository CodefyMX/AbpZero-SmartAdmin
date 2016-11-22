using Abp.Domain.Services;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.LocalizableContent
{
    public interface ILocalizableContentManager<T, TContentType> : IDomainService where T : class where TContentType : class
    {

        Task<LocalizationContentResult> CreateLocalizationContent(ILocalizableContent<T, TContentType> input, bool overwrite = false, int? tenantId = null);
        Task<Entities.AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang);
        Task<Entities.AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang, Type dtoType);
        Task<IEnumerable<Entities.AbpCinotamLocalizableContent>> GetLocalizableContent(T entity);
        Task DeleteContent(AbpCinotamLocalizableContent content);
    }
}

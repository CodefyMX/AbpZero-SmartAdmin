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
        Task<AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang);
        Task<AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang, Type dtoType);
        Task<IEnumerable<AbpCinotamLocalizableContent>> GetLocalizableContent(T entity);
        Task DeleteContent(AbpCinotamLocalizableContent content);
        Task<IEnumerable<AbpCinotamLocalizableContent>> SearchAsync(string[] lookInProperties, string search);
        Task<object> GetContentTypeAsync(T entity, string lang);
    }
}

using Abp.Domain.Services;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using System;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.LocalizableContent
{
    public interface ILocalizableContentManager<T, TContentType> : IDomainService where T : class where TContentType : class
    {

        Task<LocalizationContentResult> CreateLocalizationContent(ILocalizableContent<T, TContentType> input, int? tenantId = null);
        Task<Entities.AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang);
        Task<Entities.AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang, Type dtoType);
    }
}

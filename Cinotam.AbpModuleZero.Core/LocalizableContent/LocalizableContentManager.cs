using Abp.Domain.Services;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using Cinotam.AbpModuleZero.LocalizableContent.Store;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.LocalizableContent
{
    public class LocalizableContentManager<T, TContentType>
        : DomainService, ILocalizableContentManager<T, TContentType>
        where T : class
        where TContentType : class
    {


        private readonly ILocalizationContentStore _localizationContentStore;

        public LocalizableContentManager(ILocalizationContentStore localizationContentStore)
        {
            _localizationContentStore = localizationContentStore;
        }

        public async Task<LocalizationContentResult> CreateLocalizationContent(ILocalizableContent<T, TContentType> input, int? tenantId = null)
        {
            var cont = AbpCinotamLocalizableContent.CreateLocalizableContent(input);

            var languageIsTaken =
                _localizationContentStore.LocalizableContents.FirstOrDefault(
                    a =>
                        a.Lang == input.Lang && a.EntityDtoName == input.EntityDtoName &&
                        a.EntityName == input.EntityName && a.EntityId == input.EntityId) != null;
            if (languageIsTaken) return LocalizationContentResult.ContentExists;
            var id = await _localizationContentStore.SaveContent(cont, tenantId);
            if (id != 0)
            {
                return LocalizationContentResult.Success;
            }
            return LocalizationContentResult.Error;
        }

        public async Task<AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang)
        {
            var queryObj = QueryObj.CreateQueryObj(entity);

            var dtoInfo = QueryObj.CreateQueryObj(typeof(TContentType));

            var content = await Task.FromResult(_localizationContentStore.LocalizableContents
                .FirstOrDefault(a => a.EntityId == queryObj.EntityId
                && a.EntityName == queryObj.EntityName
                && a.EntityDtoName == dtoInfo.EntityName && lang == a.Lang));

            return content;
        }

        public async Task<AbpCinotamLocalizableContent> GetLocalizableContent(T entity, string lang, Type dtoType)
        {
            var queryObj = QueryObj.CreateQueryObj(entity);
            var dtoInfo = QueryObj.CreateQueryObj(dtoType);
            var content = await Task.FromResult(_localizationContentStore.LocalizableContents
                .FirstOrDefault(a => a.EntityId == queryObj.EntityName
                && a.EntityId == queryObj.EntityId
                && a.EntityDtoName == dtoInfo.EntityName && lang == a.Lang));

            return content;

        }
    }
}

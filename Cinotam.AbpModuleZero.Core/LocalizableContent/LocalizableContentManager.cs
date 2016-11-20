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

        public async Task CreateLocalizationContent(ILocalizableContent<T, TContentType> input, int? tenantId = null)
        {
            var cont = AbpCinotamLocalizableContent.CreateLocalizableContent(input);
            await _localizationContentStore.SaveContent(cont, tenantId);
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

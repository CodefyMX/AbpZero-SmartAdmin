using Abp.Domain.Services;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using Cinotam.AbpModuleZero.LocalizableContent.Store;
using Cinotam.AbpModuleZero.Tools.Extensions;
using System;
using System.Collections.Generic;
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

        public async Task<LocalizationContentResult> CreateLocalizationContent(ILocalizableContent<T, TContentType> input, bool overwrite, int? tenantId = null)
        {
            var cont = AbpCinotamLocalizableContent.CreateLocalizableContent(input);
            var languageInDb =
                            _localizationContentStore.LocalizableContents.FirstOrDefault(
                                a =>
                                    a.Lang == input.Lang && a.EntityDtoName == input.EntityDtoName &&
                                    a.EntityName == input.EntityName && a.EntityId == input.EntityId);
            if (languageInDb != null && !overwrite) return LocalizationContentResult.ContentExists;

            if (overwrite && languageInDb != null)
            {
                languageInDb.Properties = cont.Properties;
                await _localizationContentStore.SaveContent(languageInDb, tenantId);
                return LocalizationContentResult.ContentUpdated;
            }
            var id = await _localizationContentStore.SaveContent(cont, tenantId);
            return id != 0 ? LocalizationContentResult.Success : LocalizationContentResult.Error;
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

        public async Task<IEnumerable<AbpCinotamLocalizableContent>> GetLocalizableContent(T entity)
        {
            var queryObj = QueryObj.CreateQueryObj(entity);
            var dtoInfo = QueryObj.CreateQueryObj(typeof(TContentType));
            var contents =
                await
                    Task.FromResult(
                        _localizationContentStore.LocalizableContents.Where(a =>
                        a.EntityId == queryObj.EntityId
                        && a.EntityName == queryObj.EntityName
                        && a.EntityDtoName == dtoInfo.EntityName));
            return contents;
        }

        public Task DeleteContent(AbpCinotamLocalizableContent content)
        {
            return _localizationContentStore.RemoveContent(content);
        }

        public async Task<IEnumerable<AbpCinotamLocalizableContent>> SearchAsync(string[] lookInProperties, string search)
        {
            var elements = await Task.FromResult(_localizationContentStore.LocalizableContents);
            var result = new List<AbpCinotamLocalizableContent>();
            foreach (var abpCinotamLocalizableContent in elements)
            {
                var deserealized = LocalizableContent<T, TContentType>.DeserializeContent(abpCinotamLocalizableContent.Properties);

                result.AddRange(from lookInProperty
                                in lookInProperties
                                where deserealized.HasProperty(lookInProperty)
                                select deserealized.GetType().GetProperty(lookInProperty).GetValue(deserealized)
                                into propertyValue
                                where propertyValue != null
                                where propertyValue.ToString().Contains(search)
                                select abpCinotamLocalizableContent);


                /* Or
                 
             foreach (var lookInProperty in lookInProperties)
                {
                    if (!ClassCheckers.HasProperty(lookInProperty, deserealized)) continue;
                    var propertyValue = deserealized.GetType().GetProperty(lookInProperty).GetValue(deserealized);

                    if(propertyValue == null) continue;
                    if (propertyValue.ToString().Contains(search))
                    {
                        result.Add(abpCinotamLocalizableContent);
                    }
                }
             
             */

            }
            return result;
        }

        public async Task<object> GetContentTypeAsync(T entity, string lang)
        {
            var localizableContent = await GetLocalizableContent(entity, lang);
            return LocalizableContent<T, TContentType>.DeserializeContent(localizableContent.Properties);
        }

    }
}

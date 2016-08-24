using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.Localization.Dictionaries.Xml;
using Cinotam.AbpModuleZero.Localization.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cinotam.AbpModuleZero.Localization
{
    public class LanguageTextsProvider : DomainService, ILanguageTextsProvider
    {
        private readonly IRepository<ApplicationLanguageText, long> _languageTextsRepository;



        public LanguageTextsProvider(IRepository<ApplicationLanguageText, long> languageTextsRepository)
        {
            _languageTextsRepository = languageTextsRepository;
        }

        /// <summary>
        /// Gets all strings for the requested language
        /// </summary>
        /// <param name="sourceLang"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<LocalizedString> GetLocalizationStringFromAssembly(string sourceLang, string source)
        {

            var provider = new XmlEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                XmlLocations.GetXmlLocationBySourceName(source)
                );
            provider.Initialize(source);
            var result = new List<LocalizedString>();
            var localizationDictionary =
                provider.Dictionaries.FirstOrDefault(a => a.Value.CultureInfo.Name == sourceLang);
            var localizedStrings = localizationDictionary.Value.GetAllStrings().ToList();
            //Else we load by the source
            foreach (var localizedString in localizedStrings)
            {

                result.Add(localizedString);
            }
            return result;

        }

        public void SetLocalizationKeys(string langCode, int? tenantId)
        {
            foreach (var localizationSource in Helpers.LocalizationSources.LocalizationSourceNames)
            {
                SetLocalizationKeys(langCode, localizationSource, tenantId);
            }
        }

        private List<string> GetLocalizationKeysFromAssembly(string source)
        {
            var provider = new XmlEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                XmlLocations.GetXmlLocationBySourceName(source)
                );
            provider.Initialize(source);
            //Default dictionary = "en" en should be always available
            var strings = provider.DefaultDictionary.GetAllStrings();
            var result = strings.Select(a => a.Name).ToList();

            return result;
        }

        /// Sets the localization keys for the new language
        /// For now this works only for  
        ///  AbpModuleZeroConsts.LocalizationSourceName
        ///  it should be adapted for all the localization sources
        /// <param name="langCode"></param>
        /// <param name="source"></param>
        /// <param name="tenantId"></param>
        public void SetLocalizationKeys(string langCode, string source, int? tenantId)
        {
            if (!IsXMLAvailableForTheLangCode(langCode, source))
            {
                var keys = GetLocalizationKeysFromAssembly(source);

                foreach (var key in keys)
                {
                    _languageTextsRepository.Insert(new ApplicationLanguageText()
                    {
                        Key = key,
                        Value = string.Empty,
                        LanguageName = langCode,
                        Source = source,
                        TenantId = tenantId,
                    });
                }
                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                var keys = GetLocalizationStringFromAssembly(langCode, source);

                foreach (var key in keys)
                {
                    _languageTextsRepository.Insert(new ApplicationLanguageText()
                    {
                        Key = key.Name,
                        Value = key.Value,
                        LanguageName = langCode,
                        Source = source,
                        TenantId = tenantId,
                    });
                }
                CurrentUnitOfWork.SaveChanges();
            }
        }

        public List<string> GetLocalizationSources()
        {
            return Helpers.LocalizationSources.LocalizationSourceNames.ToList();
        }

        private bool IsXMLAvailableForTheLangCode(string langCode, string source)
        {
            var provider = new XmlEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                XmlLocations.GetXmlLocationBySourceName(source)
                );
            provider.Initialize(source);
            //Default dictionary = "en" en should be always available
            var result = provider.Dictionaries.Any(a => a.Value.CultureInfo.Name == langCode);
            return result;
        }
    }
}

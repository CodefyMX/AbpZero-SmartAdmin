using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.Localization.Dictionaries.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cinotam.AbpModuleZero.Localization
{
    public class LanguageTextsProvider : DomainService, ILanguageTextsProvider
    {
        private readonly IRepository<ApplicationLanguageText, long> _languageTextsRepository;
        private const string XmlLocation = "Cinotam.AbpModuleZero.Localization.Source";
        public LanguageTextsProvider(IRepository<ApplicationLanguageText, long> languageTextsRepository)
        {
            _languageTextsRepository = languageTextsRepository;
        }
        /// <summary>
        /// Gets all strings for the requested language
        /// </summary>
        /// <param name="sourceLang"></param>
        /// <returns></returns>
        public List<LocalizedString> GetLocalizationStringFromAssembly(string sourceLang)
        {
            var provider = new XmlEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                XmlLocation
                );
            provider.Initialize(AbpModuleZeroConsts.LocalizationSourceName);
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

        private List<string> GetLocalizationKeysFromAssembly()
        {
            var provider = new XmlEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                XmlLocation
                );
            provider.Initialize(AbpModuleZeroConsts.LocalizationSourceName);
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
        /// <param name="tenantId"></param>
        public void SetLocalizationKeys(string langCode, int? tenantId)
        {
            if (!IsXMLAvailableForTheLangCode(langCode))
            {
                var keys = GetLocalizationKeysFromAssembly();

                foreach (var key in keys)
                {
                    _languageTextsRepository.Insert(new ApplicationLanguageText()
                    {
                        Key = key,
                        Value = string.Empty,
                        LanguageName = langCode,
                        Source = AbpModuleZeroConsts.LocalizationSourceName,
                        TenantId = tenantId,
                    });
                }
                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                var keys = GetLocalizationStringFromAssembly(langCode);

                foreach (var key in keys)
                {
                    _languageTextsRepository.Insert(new ApplicationLanguageText()
                    {
                        Key = key.Name,
                        Value = key.Value,
                        LanguageName = langCode,
                        Source = AbpModuleZeroConsts.LocalizationSourceName,
                        TenantId = tenantId,
                    });
                }
                CurrentUnitOfWork.SaveChanges();
            }
        }

        private bool IsXMLAvailableForTheLangCode(string langCode)
        {
            var provider = new XmlEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                XmlLocation
                );
            provider.Initialize(AbpModuleZeroConsts.LocalizationSourceName);
            //Default dictionary = "en" en should be always available
            var result = provider.Dictionaries.Any(a => a.Value.CultureInfo.Name == langCode);
            return result;
        }
    }
}

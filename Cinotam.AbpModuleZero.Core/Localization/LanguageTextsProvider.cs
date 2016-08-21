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
            var LocalizedStrings = localizationDictionary.Value.GetAllStrings().ToList();
            //Else we load by the source
            foreach (var localizedString in LocalizedStrings)
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
            var strings = provider.DefaultDictionary.GetAllStrings();
            var result = strings.Select(a => a.Name).ToList();

            return result;
        }

        public void SetLocalizationTextForStaticLanguages(string staticLanguage, string defaultSource)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="abpTenantId"></param>
        /// <param name="sourceLang">From where the keys will be obtained</param>
        /// <param name="targetLang">Target language</param>
        /// <param name="source"></param>
        public void SetLocalizationStringsForStaticLanguage(int? abpTenantId, string sourceLang, string targetLang, string source)
        {
            /*Plan A
             
                1.- Find any language texts with the source lang
                2.- If found populate the default values for the tenant
             */

            /*Plan B
                There are not default values with the selected source
                1.- Return with no data?
             */

            var localizedString = GetLocalizationStringFromAssembly(sourceLang);
            //Localization strings found
            if (localizedString.Any())
            {
                foreach (var s in localizedString)
                {
                    _languageTextsRepository.Insert(new ApplicationLanguageText()
                    {
                        Key = s.Name,
                        Value = s.Value,
                        LanguageName = targetLang,
                        Source = source,
                        TenantId = abpTenantId,

                    });
                }

                CurrentUnitOfWork.SaveChanges();
            }


        }
        /// <summary>
        /// Sets the localization keys for the new language
        /// For now this works only for  
        ///  AbpModuleZeroConsts.LocalizationSourceName
        ///  it should be adapted for all the localization sources
        /// </summary>
        /// <param name="langCode"></param>
        /// <param name="tenantId"></param>
        public void SetLocalizationKeys(string langCode, int? tenantId)
        {
            var keys = GetLocalizationKeysFromAssembly();

            foreach (var key in keys)
            {
                _languageTextsRepository.Insert(new ApplicationLanguageText()
                {
                    Key = key,
                    Value = "",
                    LanguageName = langCode,
                    Source = AbpModuleZeroConsts.LocalizationSourceName,
                    TenantId = tenantId,
                });
            }
        }
    }
}

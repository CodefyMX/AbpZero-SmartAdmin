using Abp.Domain.Services;
using Abp.Localization;
using System.Collections.Generic;

namespace Cinotam.AbpModuleZero.Localization
{
    public interface ILanguageTextsProvider : IDomainService
    {
        List<LocalizedString> GetLocalizationStringFromAssembly(string sourceLang, string source);
        void SetLocalizationKeys(string langCode, int? tenantId);
        void SetLocalizationKeys(string langCode, string source, int? tenantId);
        List<string> GetLocalizationSources();
    }
}

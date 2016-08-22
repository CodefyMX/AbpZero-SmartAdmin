using Abp.Domain.Services;
using Abp.Localization;
using System.Collections.Generic;

namespace Cinotam.AbpModuleZero.Localization
{
    public interface ILanguageTextsProvider : IDomainService
    {
        List<LocalizedString> GetLocalizationStringFromAssembly(string sourceLang);
        void SetLocalizationKeys(string langCode, int? tenantId);
    }
}

using Abp.Configuration;
using Abp.Localization;
using Cinotam.AbpModuleZero;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule
{
    public class ModuleZeroSettingsProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition("Theme", "smart-style-0",description:L("ThemeSelector")),
                new SettingDefinition("WebSiteStatus", "On",description:L("Status"))
            };
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

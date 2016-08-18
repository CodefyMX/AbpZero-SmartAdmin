using Abp.Configuration;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule
{
    public class ModuleZeroSettingsProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition("Theme", "smart-style-0"),
                new SettingDefinition("WebSiteStatus", "On")
            };
        }
    }
}

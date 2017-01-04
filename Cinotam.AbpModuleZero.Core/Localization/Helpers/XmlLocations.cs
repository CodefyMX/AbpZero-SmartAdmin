using Abp.Modules;
using Abp.UI;
using Abp.Zero;
using System;

namespace Cinotam.AbpModuleZero.Localization.Helpers
{
    public static class XmlLocations
    {

        private const string XmlLocationModuleZero = "Cinotam.AbpModuleZero.Localization.SourceZero";
        private const string XmlAbp = "Abp.Localization.Sources.AbpXmlSource";
        private const string XmlAbpZero = "Abp.Zero.Zero.Localization.Source";
        public static LangLocalization GetXmlLocationBySourceName(string source)
        {
            switch (source)
            {
                case "AbpModuleZero":
                    return new LangLocalization() { Assembly = typeof(AbpModuleZeroCoreModule), LocalizationNameSpace = XmlLocationModuleZero };
                case "Abp":
                    return new LangLocalization() { Assembly = typeof(AbpModule), LocalizationNameSpace = XmlAbp };
                case "AbpZero":
                    return new LangLocalization() { Assembly = typeof(AbpZeroCoreModule), LocalizationNameSpace = XmlAbpZero };
                case "AbpWeb":
                    throw new UserFriendlyException("Abp web source is not available");
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
        }

        public class LangLocalization
        {
            public string LocalizationNameSpace { get; set; }
            public Type Assembly { get; set; }
        }
    }

}

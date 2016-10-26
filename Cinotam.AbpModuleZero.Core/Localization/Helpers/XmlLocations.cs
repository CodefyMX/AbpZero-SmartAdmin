using System;

namespace Cinotam.AbpModuleZero.Localization.Helpers
{
    public static class XmlLocations
    {

        private const string XmlLocationModuleZero = "Cinotam.AbpModuleZero.Localization.SourceZero";
        public static string GetXmlLocationBySourceName(string source)
        {
            switch (source)
            {
                case "AbpModuleZero":
                    return XmlLocationModuleZero;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
        }
    }
}

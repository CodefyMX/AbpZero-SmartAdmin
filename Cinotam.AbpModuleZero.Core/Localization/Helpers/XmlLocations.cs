using System;

namespace Cinotam.AbpModuleZero.Localization.Helpers
{
    public static class XmlLocations
    {

        private const string XmlLocationModuleZero = "Cinotam.AbpModuleZero.Localization.SourceZero";
        private const string XmlLocationCinotamSmartAdmin = "Cinotam.AbpModuleZero.Localization.SourceSmartAdmin";
        public static string GetXmlLocationBySourceName(string source)
        {
            switch (source)
            {
                case "AbpModuleZero":
                    return XmlLocationModuleZero;
                case "CinotamSmartAdmin":
                    return XmlLocationCinotamSmartAdmin;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
        }
    }
}

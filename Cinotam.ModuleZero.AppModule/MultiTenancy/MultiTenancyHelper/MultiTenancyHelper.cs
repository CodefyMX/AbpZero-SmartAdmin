using Abp.Extensions;
using System.Linq;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy.MultiTenancyHelper
{
    public class MultiTenancyHelper : IMultiTenancyHelper
    {

        public string GetCurrentTenancyName(string absoluteUrl)
        {
            var tenancyName = GetTenancyNameByUrl(absoluteUrl);
            return tenancyName;
        }
        private string GetTenancyNameByUrl(string absoluteUri)
        {
            if (absoluteUri.IsNullOrEmpty()) return string.Empty;

            if (absoluteUri.StartsWith("www"))
            {
                //Little cheat
                absoluteUri = "http://" + absoluteUri;
            }
            var tenancyName = absoluteUri.Split(".").First(a => !a.Contains("http") || !a.Contains("www"));
            tenancyName = tenancyName.Split("//").Last();
            return tenancyName;
        }
    }
}

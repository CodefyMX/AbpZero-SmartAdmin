using System.Web;

namespace Cinotam.AbpModuleZero.Tools.Extensions
{
    public static class ServerHelpers
    {
        public static string GetServerUrl(HttpRequestBase request)
        {
            if (request.Url == null) return string.Empty;
            var strPathAndQuery = request.Url.PathAndQuery;
            var strUrl = request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            strUrl = strUrl.Remove(strUrl.Length - 1);
            return strUrl;
        }
    }
}

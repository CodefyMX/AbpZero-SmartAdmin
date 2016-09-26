using System.Text.RegularExpressions;

namespace Cinotam.AbpModuleZero.Extensions
{
    public static class HtmlCleaner
    {
        public static string CleanHtml(string html)
        {
            var rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            var rRemCss = new Regex(@"<link[^>]*>[\s\S]*?/>");
            var clearedFromScript = rRemScript.Replace(html, "");
            var clearedFromCss = rRemCss.Replace(clearedFromScript, "");
            return clearedFromCss;
        }
    }
}

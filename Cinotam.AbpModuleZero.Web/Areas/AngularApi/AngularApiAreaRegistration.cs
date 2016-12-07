using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.AngularApi
{
    public class AngularApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AngularApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AngularApi_default",
                "AngularApi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Cinotam.AbpModuleZero.Web.Areas.AngularApi.Controllers" }
            );
        }
    }
}
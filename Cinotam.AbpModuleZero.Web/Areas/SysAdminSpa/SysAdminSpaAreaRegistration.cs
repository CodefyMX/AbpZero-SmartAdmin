using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdminSpa
{
    public class SysAdminSpaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SysAdminSpa";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SysAdminSpa_default",
                "SysAdminSpa/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Spa()
        {
            return View("~/App/SysAdmin/Main/modules/web/layout/layout.cshtml");
        }
    }
}
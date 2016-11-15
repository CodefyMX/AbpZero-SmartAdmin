using Cinotam.AbpModuleZero.Web.Controllers;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdminSpa.Controllers
{
    public class MainController : AbpModuleZeroControllerBase
    {
        // GET: SysAdminSpa/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}
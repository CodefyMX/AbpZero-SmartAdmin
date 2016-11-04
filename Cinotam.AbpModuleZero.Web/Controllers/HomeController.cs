using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{

    public class HomeController : AbpModuleZeroControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
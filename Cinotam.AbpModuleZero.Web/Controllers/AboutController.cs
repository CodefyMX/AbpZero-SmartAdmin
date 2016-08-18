using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    public class AboutController : AbpModuleZeroControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
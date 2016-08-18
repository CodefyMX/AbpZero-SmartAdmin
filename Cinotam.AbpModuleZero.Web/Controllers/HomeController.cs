using Abp.Web.Mvc.Authorization;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : AbpModuleZeroControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
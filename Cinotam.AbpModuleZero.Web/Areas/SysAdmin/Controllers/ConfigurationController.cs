using Cinotam.AbpModuleZero.Web.Controllers;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class ConfigurationController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Configuration
        public ActionResult Index()
        {
            return View();
        }
    }
}
using Cinotam.AbpModuleZero.Web.Controllers;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class CmsController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Cms
        public ActionResult MyPages()
        {
            return View();
        }
    }
}
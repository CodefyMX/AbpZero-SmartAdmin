using Cinotam.AbpModuleZero.Web.Controllers;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class PageCategoriesController : AbpModuleZeroControllerBase
    {
        public ActionResult MyCategories()
        {
            return View();
        }
    }
}
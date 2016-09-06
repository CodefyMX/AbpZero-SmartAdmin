using Cinotam.AbpModuleZero.Web.Controllers;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class NotificationsController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Notifications
        public ActionResult MyNotifications()
        {
            return View();
        }
    }
}
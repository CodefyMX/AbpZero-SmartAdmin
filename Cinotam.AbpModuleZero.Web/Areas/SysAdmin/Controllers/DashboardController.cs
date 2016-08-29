using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.AuditLogs;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesDashboard)]
    public class DashboardController : AbpModuleZeroControllerBase
    {
        private IAuditLogService _auditLogService;

        public DashboardController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }
        [DisableAuditing]
        public ActionResult Index()
        {
            return View();
        }
        [DisableAuditing]
        public ViewResult GetLogsStadistics()
        {
            return View();
        }

    }
}
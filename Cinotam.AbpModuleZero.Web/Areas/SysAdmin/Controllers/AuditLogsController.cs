using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.AuditLogs;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class AuditLogsController : AbpModuleZeroControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        // GET: SysAdmin/AuditLogs
        public ActionResult AuditLogsList(long? id)
        {
            if (id.HasValue)
            {
                ViewBag.StartupId = id.Value;
            }
            return View();
        }

        public async Task<ActionResult> GetLatestLogs()
        {
            var logs = await _auditLogService.GetLatestAuditLogOutput();
            return View(logs);
        }
    }
}
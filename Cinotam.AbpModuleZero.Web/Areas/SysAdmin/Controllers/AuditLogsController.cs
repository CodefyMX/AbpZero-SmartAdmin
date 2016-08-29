using Abp.Auditing;
using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.AuditLogs;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    [AbpMvcAuthorize(PermissionNames.AuditLogs)]
    public class AuditLogsController : AbpModuleZeroControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }
        [DisableAuditing]
        // GET: SysAdmin/AuditLogs
        public ActionResult AuditLogsList(long? id)
        {
            ViewBag.StartupId = id ?? 0;
            return View();
        }
        [DisableAuditing]
        [WrapResult(false)]
        public async Task<ActionResult> LoadLogs(RequestModel<object> input)
        {
            ProccessQueryData(input, "MethodName", new[] { "MethodName", "UserName", "ClientIpAddress" });
            var result = await _auditLogService.GetAuditLogTable(input);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [DisableAuditing]
        public async Task<ActionResult> GetLatestLogs()
        {
            var logs = await _auditLogService.GetLatestAuditLogOutput();
            return View(logs);
        }
        [DisableAuditing]
        public async Task<ActionResult> AuditLogDetail(long id)
        {
            var auditLog = await _auditLogService.GetAuditLogDetails(id);
            return View(auditLog);
        }
    }
}
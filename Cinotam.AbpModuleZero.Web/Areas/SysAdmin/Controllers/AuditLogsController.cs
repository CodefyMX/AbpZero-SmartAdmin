using Abp.Auditing;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.AuditLogs;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            ProccessQueryData(input, "MethodName", new[] { "MethodName", "ServiceName", "UserName", "ClientIpAddress", "ExecutionTime", "ExecutionDuration", "BrowserInfo" });
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

        public ActionResult GetLogs()
        {
            return View();
        }
        [WrapResult(false)]
        public async Task<JsonResult> SystemLogs(RequestModel<object> requestModel)
        {
            ProccessQueryData(requestModel, "LogLevel", new[] { "LogLevel", "Date", "Time", "ThreadNumber", "LoggerName", "LogText" });
            var tableModel = await _auditLogService.GetLogsTable(requestModel);
            return Json(tableModel, JsonRequestBehavior.AllowGet);
        }
    }
}
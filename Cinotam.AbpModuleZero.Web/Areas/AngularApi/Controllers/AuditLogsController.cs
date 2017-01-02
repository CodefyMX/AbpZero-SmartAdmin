using Abp.Auditing;
using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.AuditLogs;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.AngularApi.Controllers
{
    public class AuditLogsController : AbpModuleZeroControllerBase
    {
        // GET: AngularApi/AuditLogs
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [DisableAuditing]
        [WrapResult(false)]
        public async Task<ActionResult> LoadLogs(RequestModel<object> input, string propToSearch, string[] requestedProps)
        {
            ProccessQueryData(input, propToSearch, requestedProps);
            var result = await _auditLogService.GetAuditLogTable(input);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
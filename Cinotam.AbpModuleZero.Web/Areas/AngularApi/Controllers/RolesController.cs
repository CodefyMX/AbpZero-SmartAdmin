using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Roles;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.AngularApi.Controllers
{
    public class RolesController : AbpModuleZeroControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        // GET: AngularApi/Roles
        [AbpMvcAuthorize(PermissionNames.PagesSysAdminRoles)]
        [WrapResult(false)]
        public ActionResult LoadRoles(RequestModel<object> input, string propToSearch, string[] requestedProps)
        {
            ProccessQueryData(input, propToSearch, requestedProps);
            var result = _roleAppService.GetRolesForTable(input);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Roles;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesSysAdminRoles)]
    public class RolesController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Roles
        private readonly IRoleAppService _roleAppService;

        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        public ActionResult RolesList()
        {
            return View();
        }
        [WrapResult(false)]
        public ActionResult LoadRoles(RequestModel<object> input)
        {
            ProccessQueryData(input, "DisplayName", new[] { "DisplayName", "CreationTime" });
            var result = _roleAppService.GetRolesForTable(input);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateEditRole(int? id)
        {
            var roleInput = await _roleAppService.GetRoleForEdit(id);
            return View(roleInput);
        }
    }
}
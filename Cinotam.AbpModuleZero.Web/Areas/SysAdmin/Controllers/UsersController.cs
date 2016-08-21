using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Users;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class UsersController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Users
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [WrapResult(false)]
        public ActionResult LoadUsers(RequestModel<object> input)
        {
            ProccessQueryData(input, "UserName", new[] { "", "UserName", "EmailAddress" });
            var result = _userAppService.GetUsersForTable(input);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditRoles(long? id)
        {
            var model = await _userAppService.GetRolesForUser(id);
            return View(model);
        }

        public async Task<ActionResult> CreateEditUser(long? id)
        {
            var input = await _userAppService.GetUserForEdit(id);
            return View(input);
        }
        [HttpPost]
        public async Task<ActionResult> CreateEditUser(CreateUserInput input)
        {
            await _userAppService.CreateUser(input);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
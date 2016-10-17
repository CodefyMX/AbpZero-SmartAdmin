using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
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

        [AbpMvcAuthorize(PermissionNames.PagesSysAdminUsers)]
        public ActionResult UsersList(long? userId)
        {
            ViewBag.UserId = userId ?? 0;
            return View();
        }
        [WrapResult(false)]

        [AbpMvcAuthorize(PermissionNames.PagesSysAdminUsers)]
        public ActionResult LoadUsers(RequestModel<object> input)
        {
            ProccessQueryData(input, "UserName", new[] { "", "UserName", "EmailAddress" });
            var result = _userAppService.GetUsersForTable(input);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AbpMvcAuthorize(PermissionNames.PagesSysAdminRolesEdit)]
        public async Task<ActionResult> EditRoles(long? id)
        {
            var model = await _userAppService.GetRolesForUser(id);
            return View(model);
        }

        [AbpMvcAuthorize(PermissionNames.PagesSysAdminUsersCreate)]
        public async Task<ActionResult> CreateEditUser(long? id)
        {
            var input = await _userAppService.GetUserForEdit(id);
            return View(input);
        }

        public async Task<ActionResult> UserSpecialPermissions(long? id)
        {
            var userPermissions = await _userAppService.GetUserSpecialPermissions(id);
            return View(userPermissions);
        }
        [AbpMvcAuthorize(PermissionNames.PagesSysAdminUsersCreate)]
        [HttpPost]
        public async Task<ActionResult> CreateEditUser(CreateUserInput input)
        {
            await _userAppService.CreateUser(input);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [AbpMvcAuthorize]
        public async Task<ActionResult> MyProfile()
        {
            var user = await _userAppService.GetUserProfile(AbpSession.UserId);
            return View(user);
        }
        [AbpMvcAuthorize]
        public async Task<ActionResult> ChangeProfilePicture(long id)
        {
            if (Request.Files.Count <= 0 || Request.Files.Count > 1)
            {
                throw new UserFriendlyException("");
            }
            var result = await _userAppService.AddProfilePicture(new UpdateProfilePictureInput()
            {
                Image = Request.Files[0],
                UserId = id
            });

            return Json(result);

        }

        [AbpMvcAuthorize]
        public ActionResult Settings()
        {
            return View();
        }

        [AbpMvcAuthorize]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordInput()
            {
                UserId = AbpSession.UserId
            });
        }
    }
}
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.FileManager.Service.AppService;
using Cinotam.FileManager.Service.AppService.Dto;
using Cinotam.ModuleZero.AppModule.Users;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class UsersController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Users
        private readonly IUserAppService _userAppService;
        private readonly IFileManagerAppService _filemanagerAppService;
        public UsersController(IUserAppService userAppService, IFileManagerAppService filemanagerAppService)
        {
            _userAppService = userAppService;
            _filemanagerAppService = filemanagerAppService;
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

            var saveFile = await _filemanagerAppService.SaveFile(new SaveFileInput(Request.Files[0])
            {
                Properties =
                {
                    ["Width"] = 120,
                    ["Height"] = 120,
                    ["TransformationType"] = 2
                },
            });


            await _userAppService.AddProfilePicture(new UpdateProfilePictureInput()
            {
                ImageUrl = saveFile.Url,
                StoredInCdn = saveFile.StoredInCloud,
                UserId = id
            });

            return Json(saveFile.Url);

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
        [AbpMvcAuthorize]
        public ActionResult ChangePasswordFromAdmin(long id)
        {
            return View(new ChangePasswordInput()
            {
                UserId = id
            });
        }
        [AbpMvcAuthorize]
        public ActionResult AddEditPhoneNumber()
        {
            if (AbpSession.UserId != null) return View(new AddPhoneNumberInput() { UserId = AbpSession.UserId.Value });
            throw new NotAuthorizedException();
        }

        [AbpMvcAuthorize]
        public ActionResult ConfirmPhone(string phoneNumber, long userId, string countryCode, string countryPhoneCode)
        {
            return View(new PhoneConfirmationInput()
            {
                PhoneNumber = phoneNumber,
                UserId = userId,
                CountryPhoneCode = countryPhoneCode,
                CountryCode = countryCode

            });
        }
    }
}
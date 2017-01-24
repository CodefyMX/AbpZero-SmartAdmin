using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.FileManager.Service.AppService;
using Cinotam.FileManager.Service.AppService.Dto;
using Cinotam.ModuleZero.AppModule.Users;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.AngularApi.Controllers
{
    public class MyProfileController : AbpModuleZeroControllerBase
    {
        // GET: AngularApi/MyProfile
        private readonly IFileManagerAppService _fileManagerAppService;
        private readonly IUserAppService _userAppService;
        public MyProfileController(IFileManagerAppService fileManagerAppService, IUserAppService userAppService)
        {
            _fileManagerAppService = fileManagerAppService;
            _userAppService = userAppService;
        }

        [AbpMvcAuthorize]
        public async Task<ActionResult> ChangeProfilePicture(long id)
        {
            if (Request.Files.Count <= 0 || Request.Files.Count > 1)
            {
                throw new UserFriendlyException("");
            }

            var saveFile = await _fileManagerAppService.SaveFile(new SaveFileInput(Request.Files[0])
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
    }
}
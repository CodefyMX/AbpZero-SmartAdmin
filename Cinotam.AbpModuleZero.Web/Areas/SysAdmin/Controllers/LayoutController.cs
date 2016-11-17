using Abp.Application.Navigation;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Users;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.AbpModuleZero.Web.Models.Layout;
using Cinotam.ModuleZero.AppModule.Sessions;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class LayoutController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Layout
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly ILanguageManager _languageManager;
        private readonly UserManager _userManager;
        public LayoutController(
            IUserNavigationManager userNavigationManager,
            ISessionAppService sessionAppService, ILanguageManager languageManager, UserManager userManager)
        {
            _userNavigationManager = userNavigationManager;
            _sessionAppService = sessionAppService;
            _languageManager = languageManager;
            _userManager = userManager;
        }
        [ChildActionOnly]
        public PartialViewResult LanguageSelection()
        {
            var model = new LanguageSelectionViewModel
            {
                CurrentLanguage = _languageManager.CurrentLanguage,
                Languages = _languageManager.GetLanguages()
            };

            return PartialView("_LanguageSelectionAdmin", model);
        }

        public ViewResult UserInfo(string viewName = "_CurrentUser")
        {
            var userInfo = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations());
            return View(viewName, userInfo);
        }
        [AbpMvcAuthorize]
        public ActionResult GetNotifications()
        {

            return View();
        }

        public ActionResult ChatMenu()
        {

            var model = new List<ChatModel>();


            var users = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformationsLs());

            foreach (var getCurrentLoginInformationsOutput in users)
            {
                model.Add(new ChatModel()
                {
                    LoginInformations = getCurrentLoginInformationsOutput,
                    ConversationId = getCurrentLoginInformationsOutput.ConversationId
                });
            }
            return View(model);



        }

        public ViewResult ShouldChangePasswordMessage()
        {
            if (!AbpSession.UserId.HasValue) throw new NotAuthorizedException();
            var user = AsyncHelper.RunSync(() => _userManager.GetUserByIdAsync(AbpSession.UserId.Value));
            return View(user.ShouldChangePasswordOnLogin);
        }

        public ActionResult GetMenu(string menuName)
        {
            var model = new TopMenuViewModel
            {
                MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync(menuName, AbpSession.ToUserIdentifier())),
                ActiveMenuItemName = menuName
            };

            return PartialView("_AsideMenu", model);
        }
    }
}
using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.AbpModuleZero.Web.Models.Layout;
using Cinotam.ModuleZero.AppModule.Sessions;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class LayoutController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Layout
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ILanguageManager _languageManager;
        public LayoutController(
            IUserNavigationManager userNavigationManager,
            ILocalizationManager localizationManager,
            ISessionAppService sessionAppService,
            IMultiTenancyConfig multiTenancyConfig, ILanguageManager languageManager)
        {
            _userNavigationManager = userNavigationManager;
            _localizationManager = localizationManager;
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
            _languageManager = languageManager;
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
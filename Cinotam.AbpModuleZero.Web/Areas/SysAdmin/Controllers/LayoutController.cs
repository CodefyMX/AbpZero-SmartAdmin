using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Localization;
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

        public LayoutController(
            IUserNavigationManager userNavigationManager,
            ILocalizationManager localizationManager,
            ISessionAppService sessionAppService,
            IMultiTenancyConfig multiTenancyConfig)
        {
            _userNavigationManager = userNavigationManager;
            _localizationManager = localizationManager;
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
        }
        [ChildActionOnly]
        public PartialViewResult LanguageSelection()
        {
            var model = new LanguageSelectionViewModel
            {
                CurrentLanguage = _localizationManager.CurrentLanguage,
                Languages = _localizationManager.GetAllLanguages()
            };

            return PartialView("_LanguageSelectionAdmin", model);
        }

        public ViewResult UserInfo()
        {
            var userInfo = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations());
            return View("_CurrentUser", userInfo);
        }
        [AbpMvcAuthorize]
        public ActionResult GetNotifications()
        {

            return View();
        }
    }
}
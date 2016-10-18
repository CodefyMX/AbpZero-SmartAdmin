using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Models;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Settings;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesSysAdminConfiguration)]
    public class ConfigurationController : AbpModuleZeroControllerBase
    {
        private readonly ISettingsAppService _settingsAppService;

        public ConfigurationController(ISettingsAppService settingsAppService)
        {
            _settingsAppService = settingsAppService;
        }

        // GET: SysAdmin/Configuration
        public ActionResult Configurations()
        {
            return View();
        }

        public ActionResult GetConfigurationsByName(GetConfigurationsByNameInput input)
        {
            ViewBag.ConfigurationName = input.ConfigurationName;
            var settings = AsyncHelper.RunSync(() => _settingsAppService.GetSettingsOptions(input.SettingNames));
            return View("_configurationsView", settings);
        }
        public ActionResult GetAllConfigurations()
        {
            var settings = AsyncHelper.RunSync(() => _settingsAppService.GetSettingsOptions());
            return View("_configurationsView", settings);
        }
    }
}
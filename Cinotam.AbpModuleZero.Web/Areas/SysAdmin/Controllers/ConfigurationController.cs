using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Settings;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class ConfigurationController : AbpModuleZeroControllerBase
    {
        private readonly ISettingsAppService _settingsAppService;

        public ConfigurationController(ISettingsAppService settingsAppService)
        {
            _settingsAppService = settingsAppService;
        }

        // GET: SysAdmin/Configuration
        public async Task<ActionResult> Index()
        {
            var allSettings = await _settingsAppService.GetSettingsOptions();
            return View(allSettings);
        }
    }
}
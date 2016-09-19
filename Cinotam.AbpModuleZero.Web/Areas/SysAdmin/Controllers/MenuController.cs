using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.Cms.App.Menus;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class MenuController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Menu
        private readonly IMenuAppService _menuAppService;

        public MenuController(IMenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }

        public async Task<ActionResult> MyMenus()
        {
            var menu = await _menuAppService.GetMenuList();
            return View(menu);
        }

        public async Task<ActionResult> CreateEditMenu(int? id)
        {
            var model = await _menuAppService.GetMenuInputForEdit(id);
            return View(model);
        }

        public async Task<ActionResult> CreateEditMenuContent(int? id)
        {
            var model = await _menuAppService.GetMenuContentInputForEdit(id);
            return View(model);
        }
    }
}
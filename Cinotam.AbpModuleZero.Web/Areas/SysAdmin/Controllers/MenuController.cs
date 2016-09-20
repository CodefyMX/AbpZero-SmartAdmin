using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.Cms.App.Menus;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class MenuController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Menu
        private readonly IMenuService _menuAppService;

        public MenuController(IMenuService menuAppService)
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
            var model = await _menuAppService.GetMenuForEdit(id);
            return View(model);
        }
    }
}
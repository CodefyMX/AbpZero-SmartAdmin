using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.Cms.App.Categories;
using Cinotam.Cms.App.Menus;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class MenuController : AbpModuleZeroControllerBase
    {
        // GET: SysAdmin/Menu
        private readonly IMenuService _menuAppService;
        private readonly ICategoryService _categoryService;
        public MenuController(IMenuService menuAppService, ICategoryService categoryService)
        {
            _menuAppService = menuAppService;
            _categoryService = categoryService;
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

        public async Task<ActionResult> SetCategories(int id)
        {
            var model = await _menuAppService.GetCategorySetModel(id);
            return View(model);
        }
    }
}
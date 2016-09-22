using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.Cms.App.Categories;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class PageCategoriesController : AbpModuleZeroControllerBase
    {
        private readonly ICategoryService _categoryService;

        public PageCategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult MyCategories()
        {
            return View();
        }

        [WrapResult(false)]
        public ActionResult GetCategories(RequestModel<object> requestModel)
        {
            ProccessQueryData(requestModel, "", new[] { "DisplayName", "Name" });
            var data = _categoryService.GetCategories(requestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateEditCategoryContent(int? id)
        {
            var categoryContentModel = await _categoryService.GetCategoryForEdit(id);
            return View(categoryContentModel);
        }


    }
}
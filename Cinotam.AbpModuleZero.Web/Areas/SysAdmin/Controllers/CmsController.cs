using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.Cms.App.Pages;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class CmsController : AbpModuleZeroControllerBase
    {
        private readonly IPagesService _pagesService;

        public CmsController(IPagesService pagesService)
        {
            _pagesService = pagesService;
        }

        // GET: SysAdmin/Cms
        public ActionResult MyPages()
        {
            return View();
        }
        [WrapResult(false)]
        public ActionResult GetPagesTable(RequestModel<object> requestModel)
        {
            ProccessQueryData(requestModel, "", new[] { "Name" });
            var data = _pagesService.GetPageList(requestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateEditPage(int? id)
        {
            var page = await _pagesService.GetPageForEdit(id);
            return View(page);
        }

        public async Task<ActionResult> PageConfig(int id)
        {
            var pageConfiguration = await _pagesService.GetPageConfigurationObject(id);
            return View(pageConfiguration);
        }

        public async Task<ActionResult> ChooseTitle(int id, string lang)
        {
            var pageTitleInfo = await _pagesService.GetPageTitleForEdit(id, lang);
            return View(pageTitleInfo);
        }

    }
}
using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.Cms.App.Pages;
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

        public ActionResult CreateEditPage(int? id)
        {
            var page = _pagesService.GetPageForEdit(id);
            return View(page);
        }
    }
}
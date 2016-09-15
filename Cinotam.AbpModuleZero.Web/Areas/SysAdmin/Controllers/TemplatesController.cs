using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.Cms.App.Templates;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class TemplatesController : AbpModuleZeroControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplatesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        // GET: SysAdmin/Templates
        public ActionResult MyTemplates()
        {

            return View();
        }
        [WrapResult(false)]
        public async Task<ActionResult> GetTemplates(RequestModel<object> input)
        {
            var data = await _templateService.GetTemplatesTable(input);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CreateEditTemplate()
        {
            var model = await _templateService.GetTemplateModelForEdit();
            return View(model);
        }

        public async Task<ActionResult> EditHtml(string id)
        {
            var template = await _templateService.GetTemplateModelForEdit(id);
            return View(template);
        }
    }
}
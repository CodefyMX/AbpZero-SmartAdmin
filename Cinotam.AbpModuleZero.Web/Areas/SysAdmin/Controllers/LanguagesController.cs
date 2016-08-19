using Abp.Web.Models;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Languages;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class LanguagesController : AbpModuleZeroControllerBase
    {
        private ILanguageAppService _languageAppService;

        public LanguagesController(ILanguageAppService languageAppService)
        {
            _languageAppService = languageAppService;
        }

        // GET: SysAdmin/Languages
        public ActionResult Index()
        {
            return View();
        }
        [WrapResult(false)]
        public ActionResult LoadLanguages(RequestModel input)
        {
            ProccessQueryData(input, "DisplayName", new[] { "Name", "CreationTime" });
            var data = _languageAppService.GetLanguagesForTable(input);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateLanguage()
        {
            return View();
        }
    }
}
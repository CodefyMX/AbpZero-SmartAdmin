using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Languages;
using Cinotam.ModuleZero.AppModule.Languages.Dto;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesSysAdminLanguages)]
    public class LanguagesController : AbpModuleZeroControllerBase
    {
        private readonly ILanguageAppService _languageAppService;

        public LanguagesController(ILanguageAppService languageAppService)
        {
            _languageAppService = languageAppService;
        }

        // GET: SysAdmin/Languages
        public ActionResult LanguagesList()
        {
            return View();
        }
        [WrapResult(false)]
        public ActionResult LoadLanguages(RequestModel<object> input)
        {
            ProccessQueryData(input, "DisplayName", new[] { "Name", "CreationTime" });
            var data = _languageAppService.GetLanguagesForTable(input);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [AbpMvcAuthorize(PermissionNames.PagesSysAdminLanguagesCreate)]
        public ActionResult CreateLanguage()
        {
            return View();
        }
        [AbpMvcAuthorize(PermissionNames.PagesSysAdminLanguagesChangeTexts)]
        public ActionResult GetLanguageTexts(string targetLang)
        {
            var languageTexts = _languageAppService.GetLanguageTextsForEditView(targetLang, "en");
            return View(languageTexts);
        }
        [WrapResult(false)]
        public ActionResult GetLanguageTextsForTable(RequestModel<LanguageTextsForEditRequest> input, string source, string targetLang, string sourceLang)
        {
            input.TypeOfRequest = new LanguageTextsForEditRequest
            {
                Source = source,
                SourceLang = sourceLang,
                TargetLang = targetLang
            };
            var table = _languageAppService.GetLocalizationTexts(input);
            return Json(table, JsonRequestBehavior.AllowGet);
        }
        [AbpMvcAuthorize(PermissionNames.PagesSysAdminLanguagesChangeTexts)]
        public ActionResult EditText(LocalizationTextInput input)
        {
            return View(input);
        }
    }
}
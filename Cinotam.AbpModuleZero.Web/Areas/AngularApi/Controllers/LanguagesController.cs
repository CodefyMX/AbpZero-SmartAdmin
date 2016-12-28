using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Languages;
using Cinotam.ModuleZero.AppModule.Languages.Dto;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.AngularApi.Controllers
{
    public class LanguagesController : AbpModuleZeroControllerBase
    {
        // GET: AngularApi/Languages
        private readonly ILanguageAppService _languageAppService;

        public LanguagesController(ILanguageAppService languageAppService)
        {
            _languageAppService = languageAppService;
        }

        [AbpMvcAuthorize(PermissionNames.PagesSysAdminLanguages)]
        [WrapResult(false)]
        public async Task<ActionResult> LoadLanguages(RequestModel<object> input, string propToSearch, string[] requestedProps)
        {
            ProccessQueryData(input, propToSearch, requestedProps);
            var result = await _languageAppService.GetLanguagesForTable(input);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [WrapResult(false)]

        public ActionResult LoadLanguageTexts(RequestModel<LanguageTextsForEditRequest> input, string propToSearch, string[] requestedProps, string source, string sourceLang, string targetLang)
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
    }
}
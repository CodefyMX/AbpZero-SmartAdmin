using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Features;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class EditionsController : AbpModuleZeroControllerBase
    {
        private readonly IFeatureService _featureService;

        public EditionsController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        // GET: SysAdmin/Editions
        public ActionResult EditionList()
        {
            var editions = _featureService.GetEditions();
            return View(editions);
        }

        public async Task<ActionResult> EditCreateEdition(int? id)
        {
            var model = await _featureService.GetEditionForEdit(id);
            return View(model);
        }
    }
}
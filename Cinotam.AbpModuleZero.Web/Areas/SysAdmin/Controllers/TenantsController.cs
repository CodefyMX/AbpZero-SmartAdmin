using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.Features;
using Cinotam.ModuleZero.AppModule.MultiTenancy;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{


    // GET: SysAdmin/Tenants
    [AbpMvcAuthorize(PermissionNames.PagesTenants)]
    public class TenantsController : AbpModuleZeroControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IFeatureService _featureService;
        public TenantsController(ITenantAppService tenantAppService, IFeatureService featureService)
        {
            _tenantAppService = tenantAppService;
            _featureService = featureService;
        }
        public ActionResult TenantsList()
        {
            var output = _tenantAppService.GetTenants();
            return View("Index", output);
        }

        public async Task<ActionResult> SetTenantEdition(int tenantId)
        {
            var model = await _tenantAppService.GetEditionsForTenant(tenantId);
            return View(model);
        }

        public async Task<ActionResult> SetTenantFeatures(int tenantId)
        {
            var model = await _tenantAppService.GetFeaturesForTenant(tenantId);
            return View(model);
        }
    }
}
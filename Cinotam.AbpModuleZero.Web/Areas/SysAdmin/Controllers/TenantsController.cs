using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.MultiTenancy;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class TenantsController : AbpModuleZeroControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        // GET: SysAdmin/Tenants
        [AbpMvcAuthorize(PermissionNames.PagesTenants)]
        public ActionResult TenantsList()
        {
            var output = _tenantAppService.GetTenants();
            return View("Index", output);
        }
    }
}
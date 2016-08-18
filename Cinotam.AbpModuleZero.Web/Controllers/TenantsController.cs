﻿using Abp.Web.Mvc.Authorization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.MultiTenancy;
using System.Web.Mvc;
using Cinotam.ModuleZero.AppModule.MultiTenancy;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesTenants)]
    public class TenantsController : AbpModuleZeroControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}
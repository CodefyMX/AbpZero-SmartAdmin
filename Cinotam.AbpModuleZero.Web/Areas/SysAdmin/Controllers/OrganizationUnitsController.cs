using Cinotam.AbpModuleZero.Web.Controllers;
using Cinotam.ModuleZero.AppModule.OrganizationUnits;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Controllers
{
    public class OrganizationUnitsController : AbpModuleZeroControllerBase
    {
        private readonly IOrganizationUnitsAppService _organizationUnitsAppService;

        public OrganizationUnitsController(IOrganizationUnitsAppService organizationUnitsAppService)
        {
            _organizationUnitsAppService = organizationUnitsAppService;
        }

        public ActionResult OrganizationUnitsList()
        {
            return View();
        }

        public async Task<ActionResult> GetOrganizationUnits()
        {
            var model = await _organizationUnitsAppService.GetOrganizationUnitsConfigModel();
            return View(model);
        }

        public ActionResult CreateEditOrganizationUnit(long? id)
        {
            var model = _organizationUnitsAppService.GetOrganizationUnitForEdit(id);
            return View(model);
        }

        public ActionResult AddOrganizationUnit(int id)
        {
            var model = _organizationUnitsAppService.GetOrganizationUnitForEdit(null);
            model.ParentId = id;
            return View(model);
        }

        public async Task<ActionResult> UsersWindow(long id)
        {
            var model = await _organizationUnitsAppService.GetUsersFromOrganizationUnit(id);
            return View(model);
        }

        public ActionResult AddUserView(long id)
        {
            ViewBag.OrgUnitId = id;
            return View();
        }
    }
}
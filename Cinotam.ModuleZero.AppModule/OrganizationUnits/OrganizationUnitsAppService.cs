using Abp.Domain.Repositories;
using Abp.Organizations;
using Cinotam.ModuleZero.AppModule.OrganizationUnits.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.OrganizationUnits
{
    public class OrganizationUnitsAppService : CinotamModuleZeroAppServiceBase, IOrganizationUnitsAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        public OrganizationUnitsAppService(OrganizationUnitManager organizationUnitManager, IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
        }

        public async Task CreateOrgUnit(OrganizationUnitInput input)
        {
            await _organizationUnitManager.CreateAsync(new OrganizationUnit(AbpSession.TenantId, input.DisplayName,
                input.ParentId));
        }

        public async Task MoveOrgUnit(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.ParentId);
        }

        public async Task AddUserToOrgUnit(AddUserToOrgUnitInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.OrgUnitId);
            await UserManager.AddToOrganizationUnitAsync(user, organizationUnit);
        }

        public async Task<OrganizationUnitsConfigViewModel> GetOrganizationUnitsConfigModel()
        {
            return new OrganizationUnitsConfigViewModel()
            {
                OrganizationUnits = await GetOrganizationUnits()
            };
        }
        private async Task<List<OrganizationUnit>> GetOrganizationUnits()
        {
            var listOrganizationUnits = new List<OrganizationUnit>();
            var parentUnits = _organizationUnitRepository.GetAllList(a => a.ParentId == null);
            foreach (var organizationUnit in parentUnits)
            {
                var children = await _organizationUnitManager.FindChildrenAsync(organizationUnit.Id, true);

                organizationUnit.Children = children;

                listOrganizationUnits.Add(organizationUnit);
            }
            return listOrganizationUnits;
        }
    }
}

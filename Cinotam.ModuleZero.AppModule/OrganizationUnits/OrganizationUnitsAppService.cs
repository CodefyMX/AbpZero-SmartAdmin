using Abp.AutoMapper;
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

        public async Task CreateOrEditOrgUnit(OrganizationUnitInput input)
        {
            if (input.Id != 0)
            {
                var orgUnit = _organizationUnitRepository.Get(input.Id);
                var updated = input.MapTo(orgUnit);
                await _organizationUnitManager.UpdateAsync(updated);
            }
            else
            {
                await _organizationUnitManager.CreateAsync(new OrganizationUnit(AbpSession.TenantId, input.DisplayName,
                input.ParentId));
            }

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

        public OrganizationUnitInput GetOrganizationUnitForEdit(int? id)
        {
            if (!id.HasValue) return new OrganizationUnitInput();
            var orgUnit = _organizationUnitRepository.Get(id.Value);
            return orgUnit.MapTo<OrganizationUnitInput>();
        }

        private async Task<List<OrganizationUnitDto>> GetOrganizationUnits()
        {
            var listOrganizationUnits = new List<OrganizationUnitDto>();
            var parentUnits = (await _organizationUnitRepository.GetAllListAsync(a => a.ParentId == null));
            foreach (var organizationUnit in parentUnits)
            {
                var orgUnit = organizationUnit.MapTo<OrganizationUnitDto>();

                orgUnit.ChildrenDto = await GetChildren(organizationUnit);

                listOrganizationUnits.Add(orgUnit);

            }
            return listOrganizationUnits;
        }

        public async Task DeleteOrganizationUnit(int organizationUnitId)
        {
            var organizationUnit = await _organizationUnitRepository.FirstOrDefaultAsync(organizationUnitId);
            if (organizationUnit == null)
            {
                return;
            }
            await _organizationUnitManager.DeleteAsync(organizationUnitId);
        }
        private async Task<List<OrganizationUnitDto>> GetChildren(OrganizationUnit organizationUnit)
        {
            var childs = new List<OrganizationUnitDto>();

            var children = await _organizationUnitManager.FindChildrenAsync(organizationUnit.Id);

            foreach (var child in children)
            {
                var childElement = child.MapTo<OrganizationUnitDto>();
                childElement.ChildrenDto = await GetChildren(child);

                childs.Add(childElement);
            }

            return childs;
        }
    }
}

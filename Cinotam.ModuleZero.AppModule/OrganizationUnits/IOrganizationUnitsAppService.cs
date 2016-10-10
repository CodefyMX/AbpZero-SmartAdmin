using Abp.Application.Services;
using Cinotam.ModuleZero.AppModule.OrganizationUnits.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.OrganizationUnits
{
    public interface IOrganizationUnitsAppService : IApplicationService
    {
        Task CreateOrEditOrgUnit(OrganizationUnitInput input);
        Task MoveOrgUnit(MoveOrganizationUnitInput input);
        Task AddUserToOrgUnit(AddUserToOrgUnitInput input);
        Task<OrganizationUnitsConfigViewModel> GetOrganizationUnitsConfigModel();
        OrganizationUnitInput GetOrganizationUnitForEdit(long? id);
        Task RemoveOrganizationUnit(long id);
        Task RemoveUserFromOrganizationUnit(AddUserToOrgUnitInput input);
        Task<UsersInOrganizationUnitOutput> GetUsersFromOrganizationUnit(long id);
    }
}

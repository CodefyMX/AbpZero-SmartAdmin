using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
        ReturnModel<RoleDto> GetRolesForTable(RequestModel<object> input);
        Task<RoleInput> GetRoleForEdit(int? id);
        Task CreateEditRole(RoleInput input);
        Task DeleteRole(int roleId);
    }
}

using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultOutput<UserListDto>> GetUsers();

        ReturnModel<UserListDto> GetUsersForTable(RequestModel<object> model);
        Task DeleteUser(long? userId);

        Task CreateUser(CreateUserInput input);
        Task<CreateUserInput> GetUserForEdit(long? userId);
        Task<RoleSelectorOutput> GetRolesForUser(long? userId);
        Task SetUserRoles(RoleSelectorInput input);
        Task<UserProfileDto> GetUserProfile(long? abpSessionUserId);

        Task<string> AddProfilePicture(UpdateProfilePictureInput input);
    }
}
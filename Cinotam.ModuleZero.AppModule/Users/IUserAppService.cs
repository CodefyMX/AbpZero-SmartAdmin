using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Notifications;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using System;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultDto<UserListDto>> GetUsers();

        ReturnModel<UserListDto> GetUsersForTable(RequestModel<object> model);
        Task DeleteUser(long? userId);

        Task CreateUser(CreateUserInput input);
        Task<CreateUserInput> GetUserForEdit(long? userId);
        Task<RoleSelectorOutput> GetRolesForUser(long? userId);
        Task SetUserRoles(RoleSelectorInput input);
        Task<UserProfileDto> GetUserProfile(long? abpSessionUserId);

        Task<string> AddProfilePicture(UpdateProfilePictureInput input);
        Task<NotificationsOutput> GetMyNotifications(UserNotificationState state = UserNotificationState.Unread, int? take = null);
        Task ChangePassword(ChangePasswordInput input);
        Task MarkAsReaded(Guid notificationId);
        Task<UserSpecialPermissionsInput> GetUserSpecialPermissions(long? userId);
        Task SetUserSpecialPermissions(UserSpecialPermissionsInput input);
        Task ResetAllPermissions(long userId);
    }
}
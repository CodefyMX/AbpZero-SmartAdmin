using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Users;
using Cinotam.FileManager.Files;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.FileManager.FileTypes;
using Cinotam.FileManager.SharedTypes.Enums;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using Cinotam.ModuleZero.Notifications.UsersAppNotifications.Sender;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Users
{
    public class UserAppService : CinotamModuleZeroAppServiceBase, IUserAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IFileStoreManager _fileStoreManager;
        private readonly UserManager _userManager;
        private readonly IUsersAppNotificationsSender _usersAppNotificationsSender;
        public UserAppService(IRepository<User, long> userRepository, IPermissionManager permissionManager, UserManager userManager, IFileStoreManager fileStoreManager, IUsersAppNotificationsSender usersAppNotificationsSender)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _userManager = userManager;
            _fileStoreManager = fileStoreManager;
            _usersAppNotificationsSender = usersAppNotificationsSender;
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await UserManager.RemoveFromRoleAsync(userId, roleName));
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        public async Task<ListResultOutput<UserListDto>> GetUsers()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultOutput<UserListDto>(
                users.MapTo<List<UserListDto>>()
                );
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        public ReturnModel<UserListDto> GetUsersForTable(RequestModel<object> model)
        {
            int totalCount;
            var query = _userRepository.GetAll();

            List<Expression<Func<User, string>>> searchs = new EditableList<Expression<Func<User, string>>>();

            searchs.Add(a => a.UserName);
            searchs.Add(a => a.EmailAddress);
            searchs.Add(a => a.Name);
            searchs.Add(a => a.Surname);

            var filterByLength = GenerateTableModel(model, query, searchs, "UserName", out totalCount);
            return new ReturnModel<UserListDto>()
            {
                draw = model.draw,
                length = model.length,
                recordsTotal = totalCount,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = query.Count(),
                data = filterByLength.Select(a => a.MapTo<UserListDto>()).ToArray(),
                recordsFiltered = filterByLength.Count()
            };
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        public async Task DeleteUser(long? userId)
        {
            if (!userId.HasValue) throw new UserFriendlyException(nameof(userId));
            var userToDelete = await _userManager.GetUserByIdAsync(userId.Value);
            await _userRepository.DeleteAsync(a => a.Id == userId);

            await
                _usersAppNotificationsSender.SendUserDeletedNotification(AbpSession.UserId, userToDelete.FullName);
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        public async Task CreateUser(CreateUserInput input)
        {
            var user = input.MapTo<User>();
            var hasher = new PasswordHasher();
            if (user.Id != 0)
            {

                var userFound = _userRepository.Get(user.Id);
                var pssw = userFound.Password;
                var modified = input.MapTo(userFound);
                if (!string.IsNullOrEmpty(input.Password))
                {
                    var checkedPassword = hasher.VerifyHashedPassword(pssw, input.Password);
                    switch (checkedPassword)
                    {
                        case PasswordVerificationResult.Failed:
                            //Is new password
                            modified.Password = hasher.HashPassword(input.Password);
                            await UserManager.UpdateAsync(modified);
                            break;
                        case PasswordVerificationResult.Success:
                            //Is old password
                            modified.Password = pssw;
                            await UserManager.UpdateAsync(modified);
                            break;
                        case PasswordVerificationResult.SuccessRehashNeeded:
                            modified.Password = hasher.HashPassword(input.Password);
                            await UserManager.UpdateAsync(modified);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    await _usersAppNotificationsSender.SendUserEditedNotification(AbpSession.UserId);
                }
                else
                {
                    modified.Password = pssw;
                    await UserManager.UpdateAsync(modified);
                }

            }
            else
            {
                user.TenantId = AbpSession.TenantId;
                user.Password = new PasswordHasher().HashPassword(input.Password);
                user.IsEmailConfirmed = true;
                CheckErrors(await UserManager.CreateAsync(user));
                await _usersAppNotificationsSender.SendUserCreatedNotification(AbpSession.UserId);
            }
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        public async Task<CreateUserInput> GetUserForEdit(long? userId)
        {
            if (!userId.HasValue) return new CreateUserInput();
            var user = await UserManager.GetUserByIdAsync(userId.Value);
            var input = user.MapTo<CreateUserInput>();
            input.Password = "";
            return input;
        }

        [AbpAuthorize(PermissionNames.PagesSysAdminUsers)]
        public async Task SetUserRoles(RoleSelectorInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            await UserManager.SetRoles(user, input.Roles);
        }

        [AbpAuthorize]
        public async Task<UserProfileDto> GetUserProfile(long? abpSessionUserId)
        {
            if (!abpSessionUserId.HasValue) throw new UserFriendlyException(nameof(abpSessionUserId));
            var user = await _userManager.GetUserByIdAsync(abpSessionUserId.Value);

            return user.MapTo<UserProfileDto>();
        }
        [AbpAuthorize]
        public async Task<string> AddProfilePicture(UpdateProfilePictureInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);


            var result = _fileStoreManager.SaveFileToCloudService(new FileSaveInput()
            {
                CreateUniqueName = false,
                File = input.Image,
                FileType = ValidFileTypes.Image,
                SpecialFolder = user.UserName.Normalize(),
                ImageEditOptions = new ImageEditOptionsRequest()
                {
                    Width = 120,
                    Height = 120,
                    TransFormationType = TransformationsTypes.ImageWithSize
                }
            });


            if (result.WasStoredInCloud)
            {
                user.ProfilePicture = result.Url;
                await UserManager.UpdateAsync(user);
                return result.Url;
            }
            var folder = $"/Content/Images/Users/{input.UserId}/profilePicture/";
            var resultLocal = _fileStoreManager.SaveFileToServer(new FileSaveInput()
            {
                CreateUniqueName = true,
                File = input.Image,
            }, folder);
            user.ProfilePicture = resultLocal.VirtualPath;
            user.IsPictureOnCdn = true;
            //await _backgroundJobManager.EnqueueAsync<ProfilePictureCdnPublisher, PublisherInputDto>(new PublisherInputDto()
            //{
            //    File = resultLocal.AbsolutePath,
            //    UserName = user.UserName,
            //    UserId = input.UserId
            //});

            await UserManager.UpdateAsync(user);
            return resultLocal.VirtualPath;
        }

        public async Task<RoleSelectorOutput> GetRolesForUser(long? userId)
        {
            if (userId == null) throw new UserFriendlyException("User id");
            var userRoles = await UserManager.GetRolesAsync(userId.Value);
            var allRoles = RoleManager.Roles.ToList();
            var checkRoles = GetActiveAndInactiveRoles(userRoles, allRoles);
            return new RoleSelectorOutput()
            {
                UserId = userId.Value,
                RoleDtos = checkRoles
            };
        }

        private IEnumerable<RoleDto> GetActiveAndInactiveRoles(IList<string> userRoles, IEnumerable<Role> allRoles)
        {
            var roleDtos = new List<RoleDto>();
            foreach (var allRole in allRoles)
            {
                roleDtos.Add(new RoleDto()
                {
                    DisplayName = allRole.DisplayName,
                    Name = allRole.Name,
                    CreationTime = allRole.CreationTime,
                    IsSelected = userRoles.Any(a => a == allRole.Name),
                    IsStatic = allRole.IsStatic
                });
            }
            return roleDtos;
        }

        private async Task<List<UserRole>> GetConvertedRoles(IEnumerable<RoleInput> roles)
        {
            var rolesCreated = new List<UserRole>();
            foreach (var roleInput in roles)
            {
                var role = await RoleManager.FindByNameAsync(roleInput.RoleName);
                if (role == null) continue;
                if (roleInput.Granted)
                {
                    rolesCreated.Add(new UserRole()
                    {
                        RoleId = role.Id,
                    });
                }
            }
            return rolesCreated;
        }
    }
}
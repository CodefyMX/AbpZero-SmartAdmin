using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Notifications;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Users;
using Cinotam.FileManager.Files;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using Cinotam.ModuleZero.MailSender.CinotamMailSender;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Inputs;
using Cinotam.ModuleZero.MailSender.TemplateManager;
using Cinotam.ModuleZero.Notifications.UsersAppNotifications.Sender;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Users
{
    public class UserAppService : CinotamModuleZeroAppServiceBase, IUserAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IFileStoreManager _fileStoreManager;
        private readonly IUsersAppNotificationsSender _usersAppNotificationsSender;
        private readonly UserNotificationManager _userNotificationManager;
        private readonly ICinotamMailSender _cinotamMailSender;
        private readonly ITemplateManager _templateManager;
        public UserAppService(IRepository<User, long> userRepository, IPermissionManager permissionManager, IFileStoreManager fileStoreManager, IUsersAppNotificationsSender usersAppNotificationsSender, UserNotificationManager userNotificationManager, ICinotamMailSender cinotamMailSender, ITemplateManager templateManager)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _fileStoreManager = fileStoreManager;
            _usersAppNotificationsSender = usersAppNotificationsSender;
            _userNotificationManager = userNotificationManager;
            _cinotamMailSender = cinotamMailSender;
            _templateManager = templateManager;
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
        public async Task<ListResultDto<UserListDto>> GetUsers()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<UserListDto>(
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
            var userToDelete = await UserManager.GetUserByIdAsync(userId.Value);
            await _userRepository.DeleteAsync(a => a.Id == userId);

            await
                _usersAppNotificationsSender.SendUserDeletedNotification((await GetCurrentUserAsync()), userToDelete);
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
                modified.Password = pssw;
                await UserManager.UpdateAsync(modified);
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
                        //Rev-02.09.2016
                        //case PasswordVerificationResult.Success:
                        //    //Is old password
                        //    modified.Password = pssw;
                        //    await UserManager.UpdateAsync(modified);
                        //    break;
                        //case PasswordVerificationResult.SuccessRehashNeeded:
                        //    modified.Password = hasher.HashPassword(input.Password);
                        //    await UserManager.UpdateAsync(modified);
                        //    break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    await _usersAppNotificationsSender.SendUserEditedNotification((await GetCurrentUserAsync()), userFound);
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
                user.IsEmailConfirmed = false;


                CheckErrors(await UserManager.CreateAsync(user));

                await CurrentUnitOfWork.SaveChangesAsync();

                await SetDefaultRoles(user);

                await _usersAppNotificationsSender.SendUserCreatedNotification((await GetCurrentUserAsync()), user);


                if (input.SendNotificationMail)
                {
                    await SendWelcomeEmail(user, input.Password);
                }
            }
        }

        private async Task SetDefaultRoles(User user)
        {
            var defaultRoles = RoleManager.Roles.Where(a => a.IsDefault).Select(a => a.Name).ToArray();

            await UserManager.AddToRolesAsync(user.Id, defaultRoles);
        }

        private async Task SendWelcomeEmail(User user, string password)
        {

            dynamic sendGridParams = BuildSendGridParams(user, password);
            var welcomeMessage = string.Format(LocalizationManager.GetString(AbpModuleZeroConsts.LocalizationSourceName, "WelcomeMessage"), user.FullName);
            var yourUserIs = string.Format(LocalizationManager.GetString(AbpModuleZeroConsts.LocalizationSourceName, "YourUserIs"), user.UserName);
            var yourDefaultPasswordIs = string.Format(LocalizationManager.GetString(AbpModuleZeroConsts.LocalizationSourceName, "YourDefaultPassword"), password);
            await _cinotamMailSender.DeliverMail(new EmailSendInput()
            {
                MailMessage = new MailMessage()
                {
                    From = new MailAddress((await SettingManager.GetSettingValueAsync("Abp.Net.Mail.DefaultFromAddress"))),
                    To = { new MailAddress(user.EmailAddress) },
                    Subject = "Welcome to Cinotam.ModuleZero",
                },
                Body = _templateManager.GetContent(TemplateType.Welcome, false, welcomeMessage, yourUserIs, yourDefaultPasswordIs),
                EncodeType = "text/html",
                ExtraParams = sendGridParams,
            });
        }

        private object BuildSendGridParams(User user, string passWord)
        {
            dynamic sendGridParams = new ExpandoObject();
            sendGridParams.TemplateId = "81448bab-8391-4a6e-971d-142d68d662ad";
            sendGridParams.EnableTemplates = false;
            sendGridParams.Substitutions = new Dictionary<string, string>()
                {
                    {":user", user.FullName},
                    {":subtitle", "Welcome to Cinotam.ModuleZero"},
                    {":website","localhost:61760" },
                    {":userName",user.UserName},
                    {":password",passWord}                };
            return sendGridParams;
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
            await _usersAppNotificationsSender.SendRoleAssignedNotification(AbpSession.TenantId, (await GetCurrentUserAsync()), user);
        }

        [AbpAuthorize]
        public async Task<UserProfileDto> GetUserProfile(long? abpSessionUserId)
        {
            if (!abpSessionUserId.HasValue) throw new UserFriendlyException(nameof(abpSessionUserId));
            var user = await UserManager.GetUserByIdAsync(abpSessionUserId.Value);
            var userProfileInfo = user.MapTo<UserProfileDto>();
            userProfileInfo.MyRoles = (await UserManager.GetRolesAsync(userProfileInfo.Id)).ToList();
            return userProfileInfo;
        }
        [AbpAuthorize]
        public async Task<string> AddProfilePicture(UpdateProfilePictureInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);


            var result = await _fileStoreManager.SaveFile(new FileSaveInput()
            {
                CreateUniqueName = false,
                File = input.Image,
                SpecialFolder = user.UserName.Normalize(),
                Properties =
                {
                    ["Width"] = 120,
                    ["Height"] = 120,
                    ["TransformationType"] = 2
                },
            }, useCdnFirst: false);
            if (result.WasStoredInCloud)
            {
                user.ProfilePicture = result.Url;
                user.IsPictureOnCdn = true;
                await UserManager.UpdateAsync(user);
                return result.Url;
            }
            user.ProfilePicture = result.VirtualPath;
            user.IsPictureOnCdn = false;
            await UserManager.UpdateAsync(user);
            return result.VirtualPath;
        }
        [AbpAuthorize]
        public async Task<NotificationsOutput> GetMyNotifications(UserNotificationState state, int? take = null)
        {
            if (AbpSession.UserId == null) return new NotificationsOutput();
            var userIdentifier = new UserIdentifier(AbpSession.TenantId,
                AbpSession.UserId.Value);
            var notifications = (await _userNotificationManager.GetUserNotificationsAsync(userIdentifier));
            notifications.AddRange((await _userNotificationManager.GetUserNotificationsAsync(userIdentifier, UserNotificationState.Read)));

            if (take.HasValue)
            {
                notifications = notifications.Take(take.Value).ToList();
            }
            return new NotificationsOutput()
            {
                Notifications = notifications.ToList()
            };
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            if (input.UserId == null) throw new UserFriendlyException(L("UserNotFound"));

            var user = await UserManager.GetUserByIdAsync(input.UserId.Value);

            var hasher = new PasswordHasher();
            if (!string.IsNullOrEmpty(input.OldPassword))
            {
                var checkedPassword = hasher.VerifyHashedPassword(user.Password, input.OldPassword);
                switch (checkedPassword)
                {
                    case PasswordVerificationResult.Failed:
                        //Is new password
                        throw new UserFriendlyException(L("InvalidPassword"));
                    case PasswordVerificationResult.Success:
                        //Is old password
                        user.Password = hasher.HashPassword(input.NewPassword);
                        await UserManager.UpdateAsync(user);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //await _usersAppNotificationsSender.SendUserEditedNotification(AbpSession.UserId, user.FullName);
            }
        }

        public async Task MarkAsReaded(Guid notificationId)
        {
            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, notificationId,
                UserNotificationState.Read);
        }

        public async Task<UserSpecialPermissionsInput> GetUserSpecialPermissions(long? userId)
        {
            var assignedPermissions = new List<AssignedPermission>();
            var allPermissions = _permissionManager.GetAllPermissions().Where(a => a.Parent == null).ToList();
            if (!userId.HasValue)
                return new UserSpecialPermissionsInput()
                {
                    AssignedPermissions = assignedPermissions
                };
            var user = await UserManager.GetUserByIdAsync(userId.Value);
            var userPermissions = (await UserManager.GetGrantedPermissionsAsync(user)).ToList();
            assignedPermissions = CheckPermissions(allPermissions, userPermissions).ToList();
            return new UserSpecialPermissionsInput()
            {
                UserId = userId,
                AssignedPermissions = assignedPermissions
            };
        }

        public async Task SetUserSpecialPermissions(UserSpecialPermissionsInput input)
        {
            if (input.UserId != null)
            {
                var user = await UserManager.GetUserByIdAsync(input.UserId.Value);
                foreach (var inputAssignedPermission in input.AssignedPermissions)
                {
                    var permission = _permissionManager.GetPermission(inputAssignedPermission.Name);
                    if (inputAssignedPermission.Granted)
                    {

                        await UserManager.GrantPermissionAsync(user, permission);
                    }
                    else
                    {
                        await UserManager.ProhibitPermissionAsync(user, permission);
                    }
                }
            }
        }

        public async Task ResetAllPermissions(long userId)
        {
            var user = await UserManager.GetUserByIdAsync(userId);
            await UserManager.ResetAllPermissionsAsync(user);
        }

        public async Task<RoleSelectorOutput> GetRolesForUser(long? userId)
        {
            if (userId == null) throw new UserFriendlyException("User id");
            var userRoles = await UserManager.GetRolesAsync(userId.Value);
            var allRoles = RoleManager.Roles.ToList();
            var checkRoles = GetActiveAndInactiveRoles(userRoles, allRoles);
            var user = await UserManager.GetUserByIdAsync(userId.Value);
            return new RoleSelectorOutput()
            {
                UserId = userId.Value,
                RoleDtos = checkRoles,
                LastModifier = (await GetLastEditedForName(user.LastModifierUserId)),
                FullName = user.FullName
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
        private IEnumerable<AssignedPermission> CheckPermissions(IEnumerable<Permission> allPermissions, ICollection<Permission> userPermissions)
        {
            var permissionsFound = new List<AssignedPermission>();
            foreach (var permission in allPermissions)
            {
                AddPermission(permissionsFound, userPermissions, permission, userPermissions.Any(a => a.Name == permission.Name));
            }
            return permissionsFound;
        }
        private void AddPermission(ICollection<AssignedPermission> permissionsFound, ICollection<Permission> userPermissions, Permission allPermission, bool granted)
        {

            var childPermissions = new List<AssignedPermission>();
            var permission = new AssignedPermission()
            {
                DisplayName = allPermission.DisplayName.Localize(new LocalizationContext(LocalizationManager)),
                Granted = granted,
                Name = allPermission.Name,
            };
            if (allPermission.Children.Any())
            {
                foreach (var childPermission in allPermission.Children.WhereIf(AbpSession.TenantId.HasValue, a => a.Name != PermissionNames.PagesTenants))
                {
                    AddPermission(childPermissions, userPermissions, childPermission, userPermissions.Any(a => a.Name == childPermission.Name));
                }

                permission.ChildPermissions.AddRange(childPermissions);
            }

            permissionsFound.Add(permission);
        }
    }
}
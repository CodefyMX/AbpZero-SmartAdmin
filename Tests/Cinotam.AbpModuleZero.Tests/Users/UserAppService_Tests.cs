using Abp.Authorization;
using Abp.Notifications;
using Abp.UI;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.EntityFramework;
using Cinotam.AbpModuleZero.Tests.FakeRequests;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.Roles;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using Cinotam.ModuleZero.AppModule.Users;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.Users
{
    public class UserAppService_Tests : AbpModuleZeroTestBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;

        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
            _roleAppService = Resolve<IRoleAppService>();
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            LoginAsDefaultTenantAdmin();
            //Act
            var output = await _userAppService.GetUsers();

            //Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldNotGetUsers_Test()
        {
            try
            {
                //Act
                var output = await _userAppService.GetUsers();

                //Assert
                output.Items.Count.ShouldBeGreaterThan(0);
            }
            catch (Exception ex)
            {
                ex.ShouldBeAssignableTo<AbpAuthorizationException>();
            }
        }

        [Fact]
        public async Task CreateUser_Test()
        {
            LoginAsDefaultTenantAdmin();
            //Act
            await CreateFakeUser();

            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);
                user.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task ShouldNotCreateUser_Test()
        {
            try
            {
                //Act
                await CreateFakeUser();
            }
            catch (Exception ex)
            {
                ex.ShouldBeAssignableTo<AbpAuthorizationException>();
            }
        }

        [Fact]
        public async Task GetUserForEdit_Test()
        {
            LoginAsDefaultTenantAdmin();
            await CreateFakeUser();

            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);

                var userEditInput = await _userAppService.GetUserForEdit(user.Id);

                userEditInput.ShouldNotBe(null);

            });
        }

        [Fact]
        public async Task ShouldBeNewUserInstance_Test()
        {
            LoginAsDefaultTenantAdmin();

            var userEditInput = await _userAppService.GetUserForEdit(null);

            userEditInput.ShouldNotBe(null);
            userEditInput.UserName.ShouldBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldNotGetUser_Test()
        {
            try
            {
                await _userAppService.GetUserForEdit(null);
            }
            catch (Exception ex)
            {
                ex.ShouldBeAssignableTo<AbpAuthorizationException>();
            }
        }

        [Fact]
        public async Task GetUserProfile_Test()
        {
            LoginAsDefaultTenantAdmin();
            await CreateFakeUser();

            await UsingDbContextAsync(async context =>
            {
                var user = GetFakeUser(context);

                var userProfile = await _userAppService.GetUserProfile(user.Id);

                userProfile.ShouldNotBe(null);
                userProfile.MyRoles.ShouldNotBe(null);
                userProfile.MyRoles.ShouldBeAssignableTo<IEnumerable>();
            });
        }

        [Fact]
        public async Task ProfileShouldThrowException_Test()
        {
            try
            {

                LoginAsDefaultTenantAdmin();
                await _userAppService.GetUserProfile(null);
            }
            catch (Exception ex)
            {
                ex.ShouldBeAssignableTo<UserFriendlyException>();
            }
        }

        [Fact]
        public async Task ShouldNotGetProfile_Test()
        {
            try
            {
                await _userAppService.GetUserProfile(null);
            }
            catch (Exception ex)
            {
                ex.ShouldBeAssignableTo<AbpAuthorizationException>();
            }
        }

        [Fact]
        public async Task GetMyNotifications_Test()
        {
            LoginAsHostAdmin();
            var userNotifications = await _userAppService.GetMyNotifications(UserNotificationState.Unread, 100);

            userNotifications.ShouldNotBe(null);

        }
        [Fact]
        public async Task ChangePassword_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeUser();

            await UsingDbContextAsync(async context =>
            {

                var user = await GetFakeUser(context);

                await _userAppService.ChangePassword(new ChangePasswordInput()
                {
                    OldPassword = "123qwe",
                    NewPassword = "qwe123",
                    UserId = user.Id
                });

            });

        }

        [Fact]
        public async Task SetUserRoles_Test()
        {
            await CreateAndAssignFakeRoleToFakeUser();
        }

        [Fact]
        public async Task DeleteUser_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeUser();
            await UsingDbContextAsync(async context =>
            {

                var user = await GetFakeUser(context);

                await _userAppService.DeleteUser(user.Id);
            });

        }
        [Fact]
        public async Task RemoveFromRole_Test()
        {
            await CreateAndAssignFakeRoleToFakeUser();

            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);

                var role = await GetFakeRole(context);

                await _userAppService.RemoveFromRole(user.Id, role.Name);
            });

        }

        [Fact]
        public async Task GetRolesForUser_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeUser();
            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);

                var roles = await _userAppService.GetRolesForUser(user.Id);

                roles.ShouldNotBe(null);

                roles.RoleDtos.ShouldNotBe(null);

                roles.RoleDtos.ShouldBeAssignableTo<IEnumerable>();
            });
        }
        [Fact]
        public void GetUsersFromTable_Test()
        {
            LoginAsHostAdmin();
            var table = _userAppService.GetUsersForTable(FakeRequestHelper.CreateDataTablesFakeRequestModel());
            table.ShouldNotBe(null);
            table.data.ShouldNotBe(null);
            table.data.Count().ShouldBe(1);
            table.data.ShouldBeAssignableTo<Array>();
        }

        [Fact]
        public async Task GetUserSpecialPermissions_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeUser();
            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);
                var permissions = await _userAppService.GetUserSpecialPermissions(user.Id);
                permissions.ShouldNotBeNull();
                permissions.AssignedPermissions.ShouldNotBeNull();
                permissions.AssignedPermissions.Count().ShouldBe(0);

                await _userAppService.SetUserSpecialPermissions(new UserSpecialPermissionsInput()
                {
                    UserId = user.Id,
                    AssignedPermissions = new List<AssignedPermission>()
                    {
                        new AssignedPermission()
                        {
                            Name = PermissionNames.PagesSysAdminRoles
                        }
                    }
                });

                permissions = await _userAppService.GetUserSpecialPermissions(user.Id);
                permissions.ShouldNotBeNull();
                permissions.AssignedPermissions.Count().ShouldBe(1);

                await _userAppService.ResetAllPermissions(user.Id);

                permissions = await _userAppService.GetUserSpecialPermissions(user.Id);
                permissions.ShouldNotBeNull();
                permissions.AssignedPermissions.Count().ShouldBe(0);

            });
        }
        private async Task CreateFakeUser()
        {
            await _userAppService.CreateUser(
                new CreateUserInput
                {
                    EmailAddress = "john@volosoft.com",
                    IsActive = true,
                    Name = "John",
                    Surname = "Nash",
                    Password = "123qwe",
                    UserName = "john.nash"
                });
        }

        private async Task<User> GetFakeUser(AbpModuleZeroDbContext context)
        {
            var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
            return johnNashUser;
        }

        private async Task CreateFakeRole()
        {
            await _roleAppService.CreateEditRole(new RoleInput()
            {
                DisplayName = "FakeRole"
            });
        }

        private async Task<Role> GetFakeRole(AbpModuleZeroDbContext context)
        {
            var fakeRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "FakeRole");
            return fakeRole;
        }
        private async Task CreateAndAssignFakeRoleToFakeUser()
        {
            LoginAsHostAdmin();
            await CreateFakeUser();
            await CreateFakeRole();
            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);

                var role = await GetFakeRole(context);

                await _userAppService.SetUserRoles(new RoleSelectorInput()
                {
                    Roles = new[] { role.Name },
                    UserId = user.Id
                });
            });
        }
    }
}

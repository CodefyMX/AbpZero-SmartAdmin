using Abp.Notifications;
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
using System.Data.Entity;
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
            //Act
            var output = await _userAppService.GetUsers();

            //Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task CreateUser_Test()
        {
            //Act
            await CreateFakeUser();

            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);
                user.ShouldNotBeNull();
            });
        }
        [Fact]
        public async Task GetUserForEdit_Test()
        {
            await CreateFakeUser();

            await UsingDbContextAsync(async context =>
            {
                var user = await GetFakeUser(context);

                var userEditInput = await _userAppService.GetUserForEdit(user.Id);

                userEditInput.ShouldNotBe(null);

            });
        }
        [Fact]
        public async Task GetUserProfile_Test()
        {
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

        //private const string FakeImage = "/Content/Images/fakeProfile.png";
        //[Fact]
        //public async Task AddProfilePicture()
        //{
        //    //http://www.hanselman.com/blog/ABackToBasicsCaseStudyImplementingHTTPFileUploadWithASPNETMVCIncludingTestsAndMocks.aspx
        //    await CreateFakeUser();

        //    using (var stream = new FileStream(HostingEnvironment.MapPath(FakeImage),
        //             FileMode.Open))
        //    {
        //        var context = new Mock<HttpContextBase>();
        //        var request = new Mock<HttpRequestBase>();
        //        var files = new Mock<HttpFileCollectionBase>();
        //        var file = new Mock<HttpPostedFileBase>();

        //        context.Setup(x => x.Request).Returns(request.Object);

        //        files.Setup(x => x.Count).Returns(1);

        //        // The required properties from my Controller side

        //        file.Setup(x => x.InputStream).Returns(stream);
        //        file.Setup(x => x.ContentLength).Returns((int)stream.Length);
        //        file.Setup(x => x.FileName).Returns(Path.GetFileName(stream.Name));
        //        files.Setup(x => x.Get(0).InputStream).Returns(file.Object.InputStream);

        //        request.Setup(x => x.Files).Returns(files.Object);
        //        request.Setup(x => x.Files[0]).Returns(file.Object);

        //        var controller = new UsersController(_userAppService);
        //        controller.ControllerContext = new ControllerContext(
        //                                 context.Object, new RouteData(), controller);

        //        await UsingDbContextAsync(async dbContext =>
        //        {
        //            var user = await GetFakeUser(dbContext);
        //            await controller.ChangeProfilePicture(user.Id);
        //        });
        //    }
        //}
        [Fact]
        public void GetUsersFromTable_Test()
        {
            LoginAsHostAdmin();
            var table = _userAppService.GetUsersForTable(FakeRequestHelper.CreateDataTablesFakeRequestModel());
            table.ShouldNotBe(null);
            table.data.ShouldNotBe(null);
            table.data.ShouldBeAssignableTo<Array>();
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

using Abp.Authorization;
using Cinotam.AbpModuleZero.EntityFramework;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.Notifications;
using Cinotam.ModuleZero.AppModule.Users;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using Shouldly;
using System;
using System.Collections;
using System.Data.Entity;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.Notifications
{
    public class NotificationService_Test : AbpModuleZeroTestBase
    {
        readonly INotificationService _notificationService;
        private readonly IUserAppService _userAppService;

        public NotificationService_Test()
        {
            _notificationService = Resolve<INotificationService>();
            _userAppService = Resolve<IUserAppService>();
        }

        [Fact]
        public async Task GetMyNotifications_Test()
        {
            LoginAsHostAdmin();
            var result = await _notificationService.GetMyNotifications(10, 0, "");
            result.ShouldNotBeNull();
            result.Notifications.ShouldNotBeNull();
            result.Notifications.ShouldBeAssignableTo<IEnumerable>();

        }

        [Fact]
        public async Task ShouldNotGetNotifications_Test()
        {
            try
            {
                await _notificationService.GetMyNotifications(10, 0, "");
            }
            catch (Exception ex)
            {
                ex.ShouldBeAssignableTo<AbpAuthorizationException>();
            }


        }
        private async Task CreateFakeUser(string userName = "john.nash", string mail = "john@volosoft.com")
        {
            await _userAppService.CreateUser(
                new CreateUserInput
                {
                    EmailAddress = mail,
                    IsActive = true,
                    Name = "John",
                    Surname = "Nash",
                    Password = "123qwe",
                    UserName = userName
                });
        }

        private async Task<User> GetFakeUser(AbpModuleZeroDbContext context, string userName = "john.nash")
        {
            var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return johnNashUser;
        }
    }
}

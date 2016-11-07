﻿using Cinotam.ModuleZero.AppModule.Sessions;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.Sessions
{
    public class SessionAppService_Tests : AbpModuleZeroTestBase
    {
        private readonly ISessionAppService _sessionAppService;

        public SessionAppService_Tests()
        {
            _sessionAppService = Resolve<ISessionAppService>();
        }

        [Fact]
        public async Task Should_Get_Current_User_When_Logged_In_As_Host()
        {

            //Arrange
            LoginAsHostAdmin();

            //Act
            var output = await _sessionAppService.GetCurrentLoginInformations();

            //Assert
            var currentUser = await GetCurrentUserAsync();
            output.User.ShouldNotBe(null);
            output.User.Name.ShouldBe(currentUser.Name);
            output.User.Surname.ShouldBe(currentUser.Surname);
            if (IsMultiTenancyEnabled)
            {

                output.Tenant.ShouldBe(null);
            }
            else
            {
                output.Tenant.ShouldNotBe(null);
            }
        }

        [Fact]
        public async Task Should_Get_Current_User_And_Tenant_When_Logged_In_As_Tenant()
        {
            LoginAsDefaultTenantAdmin();
            //Act
            var output = await _sessionAppService.GetCurrentLoginInformations();

            //Assert
            var currentUser = await GetCurrentUserAsync();
            var currentTenant = await GetCurrentTenantAsync();

            output.User.ShouldNotBe(null);
            output.User.Name.ShouldBe(currentUser.Name);

            output.Tenant.ShouldNotBe(null);
            output.Tenant.Name.ShouldBe(currentTenant.Name);
        }
    }
}

using Abp.Organizations;
using Cinotam.AbpModuleZero.EntityFramework;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.OrganizationUnits;
using Cinotam.ModuleZero.AppModule.OrganizationUnits.Dto;
using Cinotam.ModuleZero.AppModule.Users;
using Cinotam.ModuleZero.AppModule.Users.Dto;
using Shouldly;
using System.Collections;
using System.Data.Entity;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.OrganizationUnits
{
    public class OrganizationUnitsAppService_Test : AbpModuleZeroTestBase
    {
        private readonly IOrganizationUnitsAppService _organizationUnitsAppService;
        private readonly IUserAppService _userAppService;
        public OrganizationUnitsAppService_Test()
        {
            _organizationUnitsAppService = Resolve<IOrganizationUnitsAppService>();
            _userAppService = Resolve<IUserAppService>();
        }
        [Fact]
        public async Task CreateOrEditOrgUnit_Test()
        {
            await CreateFakeOrganizationUnit();
            await UsingDbContextAsync(async dbContext =>
            {
                var orgUnit = await GetFakeOrganizationUnit(dbContext);

                orgUnit.ShouldNotBeNull();
                orgUnit.DisplayName.ShouldNotBeNullOrWhiteSpace();
                orgUnit.Code.ShouldNotBeNullOrEmpty();
            });
        }
        [Fact]
        public async Task MoveOrgUnit_Test()
        {
            await CreateFakeOrganizationUnit();
            await CreateFakeOrganizationUnit("FakeOrgUnit2");
            await UsingDbContextAsync(async dbContext =>
            {
                var orgUnit1 = await GetFakeOrganizationUnit(dbContext);
                var orgUnit2 = await GetFakeOrganizationUnit(dbContext, "FakeOrgUnit2");

                await _organizationUnitsAppService.MoveOrgUnit(new MoveOrganizationUnitInput()
                {
                    Id = orgUnit2.Id,
                    ParentId = orgUnit1.ParentId
                });


                var orgUnit2Check = await GetFakeOrganizationUnit(dbContext, "FakeOrgUnit2");
                orgUnit2Check.ParentId.ShouldBe(orgUnit1.Id);
            });
        }
        [Fact]
        public async Task AddUserToOrgUnit_GetUsersFromOrganizationUnit_Test()
        {
            LoginAsDefaultTenantAdmin();
            await CreateFakeOrganizationUnit();

            await CreateFakeUser();
            await CreateFakeUser("Pancho", "Pancho@pantera.com");

            await UsingDbContextAsync(async dbContext =>
            {
                var orgUnit = await GetFakeOrganizationUnit(dbContext);

                var user = await GetFakeUser(dbContext);
                var user2 = await GetFakeUser(dbContext, "Pancho");

                await _organizationUnitsAppService.AddUserToOrgUnit(new AddUserToOrgUnitInput()
                {
                    OrgUnitId = orgUnit.Id,
                    UserId = user.Id
                });

                await _organizationUnitsAppService.AddUserToOrgUnit(new AddUserToOrgUnitInput()
                {
                    OrgUnitId = orgUnit.Id,
                    UserId = user2.Id
                });

                var output = await _organizationUnitsAppService.GetUsersFromOrganizationUnit(orgUnit.Id);

                output.ShouldNotBeNull();
                output.Users.ShouldBeAssignableTo<IEnumerable>();
                output.Users.ShouldNotBeNull();
                output.Users.Count.ShouldBe(2);

            });
        }

        [Fact]
        public async Task GetOrganizationUnitsConfigModel_Test()
        {
            await CreateFakeOrganizationUnit();
            await CreateFakeOrganizationUnit("Fake1");
            await CreateFakeOrganizationUnit("Fake2");

            var output = await _organizationUnitsAppService.GetOrganizationUnitsConfigModel();

            output.ShouldNotBeNull();
            output.OrganizationUnits.ShouldNotBeNull();
            output.OrganizationUnits.ShouldBeAssignableTo<IEnumerable>();

            output.OrganizationUnits.Count.ShouldBe(3);
        }

        [Fact]
        public async Task GetOrganizationUnitForEdit()
        {
            await CreateFakeOrganizationUnit();

            await UsingDbContextAsync(async dbContext =>
            {
                var orgUnit = await GetFakeOrganizationUnit(dbContext);

                var orgUnitInput = _organizationUnitsAppService.GetOrganizationUnitForEdit(orgUnit.Id);

                orgUnitInput.ShouldNotBeNull();
                orgUnitInput.DisplayName.ShouldNotBeNullOrWhiteSpace();
                orgUnitInput.Code.ShouldNotBeNullOrEmpty();

            });

        }

        [Fact]
        public async Task RemoveOrganizationUnit()
        {
            await CreateFakeOrganizationUnit();

            await UsingDbContextAsync(async dbContext =>
            {
                var orgUnit = await GetFakeOrganizationUnit(dbContext);

                await _organizationUnitsAppService.RemoveOrganizationUnit(orgUnit.Id);

            });
        }
        [Fact]
        public async Task RemoveUserFromOrganizationUnit()
        {
            LoginAsDefaultTenantAdmin();
            await CreateFakeOrganizationUnit();
            await CreateFakeUser();
            await UsingDbContextAsync(async dbContext =>
            {
                var orgUnit = await GetFakeOrganizationUnit(dbContext);

                var user = await GetFakeUser(dbContext);

                await _organizationUnitsAppService.AddUserToOrgUnit(new AddUserToOrgUnitInput()
                {
                    OrgUnitId = orgUnit.Id,
                    UserId = user.Id
                });

                var output = await _organizationUnitsAppService.GetUsersFromOrganizationUnit(orgUnit.Id);

                output.Users.Count.ShouldBe(1);

                await _organizationUnitsAppService.RemoveUserFromOrganizationUnit(new AddUserToOrgUnitInput()
                {
                    OrgUnitId = orgUnit.Id,
                    UserId = user.Id
                });
                var outputFinalCheck = await _organizationUnitsAppService.GetUsersFromOrganizationUnit(orgUnit.Id);

                outputFinalCheck.Users.Count.ShouldBe(0);

            });
        }
        private async Task CreateFakeOrganizationUnit(string name = "FakeOrgUnit", int? parent = null)
        {
            await _organizationUnitsAppService.CreateOrEditOrgUnit(new OrganizationUnitInput()
            {
                DisplayName = name,
                ParentId = parent
            });

        }
        private async Task<OrganizationUnit> GetFakeOrganizationUnit(AbpModuleZeroDbContext dbContext, string name = "FakeOrgUnit")
        {
            var orgUnit = await dbContext.OrganizationUnits.FirstOrDefaultAsync(a => a.DisplayName == name);
            return orgUnit;
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

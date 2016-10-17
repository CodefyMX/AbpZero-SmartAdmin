using Abp.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.EntityFramework;
using Cinotam.ModuleZero.AppModule.Roles;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using Shouldly;
using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.Roles
{
    public class RoleAppService_Tests : AbpModuleZeroTestBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IPermissionManager _permissionManager;

        public RoleAppService_Tests()
        {
            _roleAppService = Resolve<IRoleAppService>();
            _permissionManager = Resolve<IPermissionManager>();
        }


        /*
        Task DeleteRole(int roleId);
         
         */
        [Fact]
        public async Task UpdateRolePermissions_Test()
        {
            LoginAsHostAdmin();

            await CreateFakeRole();


            await UsingDbContextAsync(async dbContext =>
            {
                var role = await GetFakeRole(dbContext);

                var permissions = _permissionManager.GetAllPermissions();

                var permissionStrings = permissions.Select(a => a.Name).ToList();


                await _roleAppService.UpdateRolePermissions(new UpdateRolePermissionsInput()
                {
                    GrantedPermissionNames = permissionStrings,
                    RoleId = role.Id
                });
            });

        }
        [Fact]
        public void GetRolesForTable_Test()
        {
            LoginAsDefaultTenantAdmin();
            var fakeTableRequest = FakeRequests.FakeRequestHelper.CreateDataTablesFakeRequestModel();

            var tableModel = _roleAppService.GetRolesForTable(fakeTableRequest);

            tableModel.ShouldNotBeNull();

            tableModel.data.ShouldNotBeNull();

            tableModel.data.ShouldBeAssignableTo<Array>();

        }
        [Fact]
        public async Task GetRoleForEdit_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeRole();
            await UsingDbContextAsync(async dbContext =>
            {
                var role = await GetFakeRole(dbContext);
                var roleInput = await _roleAppService.GetRoleForEdit(role.Id);

                roleInput.ShouldNotBeNull();
                roleInput.Name.ShouldNotBeNullOrWhiteSpace();
                roleInput.DisplayName.ShouldNotBeNullOrEmpty();
                roleInput.AssignedPermissions.ShouldNotBeNull();
                roleInput.AssignedPermissions.ShouldBeAssignableTo<IEnumerable>();


            });
        }

        [Fact]
        public async Task CreateEditRole_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeRole();
            await UsingDbContextAsync(async dbContext =>
            {
                var role = await GetFakeRole(dbContext);
                role.ShouldNotBeNull();
                role.Name.ShouldNotBeNullOrWhiteSpace();
                role.DisplayName.ShouldNotBeNullOrEmpty();
            });
        }

        [Fact]
        public async Task DeleteRole_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeRole();
            await UsingDbContextAsync(async dbContext =>
            {
                var role = await GetFakeRole(dbContext);
                await _roleAppService.DeleteRole(role.Id);
            });
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
    }
}

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Editions;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.Features.Dto;
using Cinotam.ModuleZero.AppModule.MultiTenancy.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy
{
    [AbpAuthorize(PermissionNames.PagesTenants)]
    public class TenantAppService : CinotamModuleZeroAppServiceBase, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly RoleManager _roleManager;
        private readonly EditionManager _editionManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;

        public TenantAppService(
            TenantManager tenantManager,
            RoleManager roleManager,
            EditionManager editionManager,
            IAbpZeroDbMigrator abpZeroDbMigrator)
        {
            _tenantManager = tenantManager;
            _roleManager = roleManager;
            _editionManager = editionManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
        }

        public ListResultDto<TenantListDto> GetTenants()
        {
            return new ListResultDto<TenantListDto>(
                _tenantManager.Tenants
                    .OrderBy(t => t.TenancyName)
                    .ToList()
                    .MapTo<List<TenantListDto>>()
                );
        }

        public async Task EnableFeatureForTenant(EnableFeatureInput input)
        {
            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            await _tenantManager.SetFeatureValueAsync(tenant, input.FeatureName, input.FeatureStatus);
        }

        public async Task<EditionsForTenantOutput> GetEditionsForTenant(int tenantId)
        {
            var allEditions = _editionManager.Editions.ToList();
            var editionCList = new List<EditionDtoCustom>();
            foreach (var allEdition in allEditions)
            {
                var mappedEdition = allEdition.MapTo<EditionDtoCustom>();

                mappedEdition.IsEnabledForTenant = await IsThisEditionActive(tenantId, allEdition.Id);

                editionCList.Add(mappedEdition);
            }
            return new EditionsForTenantOutput()
            {
                Editions = editionCList
            };
        }

        private async Task<bool> IsThisEditionActive(int tenantId, int editionId)
        {
            var tenant = await TenantManager.GetByIdAsync(tenantId);

            return tenant.EditionId == editionId;

        }

        public async Task SetTenantEdition(SetTenantEditionInput input)
        {
            var tenant = await _tenantManager.GetByIdAsync(input.TenantId);
            var edition = await _editionManager.FindByIdAsync(input.EditionId);
            if (edition != null)
            {
                tenant.EditionId = edition.Id;
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }
        public async Task CreateTenant(CreateTenantInput input)
        {
            //Create tenant
            var tenant = input.MapTo<Tenant>();
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            CheckErrors(await TenantManager.CreateAsync(tenant));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

            //Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            //We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                //Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); //To get static role ids

                //grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                //Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, User.DefaultPassword);
                CheckErrors(await UserManager.CreateAsync(adminUser));
                await CurrentUnitOfWork.SaveChangesAsync(); //To get admin user's id

                //Assign admin user to role!
                CheckErrors(await UserManager.AddToRoleAsync(adminUser.Id, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
    }
}
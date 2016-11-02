using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Editions;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.Features.Dto;
using Cinotam.ModuleZero.AppModule.Features.FeatureManager;
using Cinotam.ModuleZero.AppModule.MultiTenancy.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy
{
    [AbpAuthorize(PermissionNames.PagesTenants)]
    public class TenantAppService : CinotamModuleZeroAppServiceBase, ITenantAppService
    {
        private readonly RoleManager _roleManager;
        private readonly EditionManager _editionManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly ICustomEditionManager _customEditionManager;

        public TenantAppService(
            RoleManager roleManager,
            EditionManager editionManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            ICustomEditionManager customEditionManager)
        {
            _roleManager = roleManager;
            _editionManager = editionManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _customEditionManager = customEditionManager;
        }

        public ListResultDto<TenantListDto> GetTenants()
        {
            return new ListResultDto<TenantListDto>(
                TenantManager.Tenants
                    .OrderBy(t => t.TenancyName)
                    .ToList()
                    .MapTo<List<TenantListDto>>()
                );
        }

        public async Task SetFeatureValuesForTenant(CustomEditionInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }

            var tenant = await TenantManager.GetByIdAsync(input.TenantId);

            foreach (var inputFeature in input.Features)
            {
                await TenantManager.SetFeatureValueAsync(tenant, inputFeature.Name, inputFeature.DefaultValue);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<CustomEditionInput> GetFeaturesForTenant(int tenantId)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }
            var tenant = await TenantManager.GetByIdAsync(tenantId);

            if (tenant.EditionId == null) throw new UserFriendlyException(L("NoEditionIsSetForTenant"));

            var edition = await _editionManager.FindByIdAsync(tenant.EditionId.Value);


            var mapped = edition.MapTo<CustomEditionInput>();

            mapped.TenantId = tenantId;

            mapped.Features = await _customEditionManager.GetAllFeatures(edition.Id, tenantId);

            return mapped;
        }


        public async Task ResetFeatures(int tenantId)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }
            var tenant = await TenantManager.GetByIdAsync(tenantId);

            if (tenant.EditionId == null) throw new UserFriendlyException(L("NoEditionIsSetForTenant"));

            var editionFeatures = await _editionManager.GetFeatureValuesAsync(tenant.EditionId.Value);

            await TenantManager.SetFeatureValuesAsync(tenantId, editionFeatures.ToArray());

        }

        public ReturnModel<TenantListDto> GetTenantsTable(RequestModel<object> input)
        {

            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }
            int count;
            var query = TenantManager.Tenants;

            List<Expression<Func<Tenant, string>>> searchs = new EditableList<Expression<Func<Tenant, string>>>();

            searchs.Add(a => a.Name);
            searchs.Add(a => a.TenancyName);
            var filteredByLength = GenerateTableModel(input, query, searchs, "Id", out count);

            return new ReturnModel<TenantListDto>()
            {
                iTotalDisplayRecords = count,
                recordsTotal = query.Count(),
                recordsFiltered = filteredByLength.Count,
                length = input.length,
                data = filteredByLength.Select(a => a.MapTo<TenantListDto>()).ToArray(),
                draw = input.draw,
            };
        }

        public async Task<TenantViewModel> GetTenantViewModel(int tenantId)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }
            var tenant = await TenantManager.GetByIdAsync(tenantId);

            return tenant.MapTo<TenantViewModel>();
        }

        public async Task<EditionsForTenantOutput> GetEditionsForTenant(int tenantId)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }
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
                Editions = editionCList,
                TenantId = tenantId
            };
        }

        private async Task<bool> IsThisEditionActive(int tenantId, int editionId)
        {

            var tenant = await TenantManager.GetByIdAsync(tenantId);

            return tenant.EditionId == editionId;

        }

        public async Task SetTenantEdition(SetTenantEditionInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }
            var tenant = await TenantManager.GetByIdAsync(input.TenantId);
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

        public async Task DeleteTenant(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            await TenantManager.DeleteAsync(tenant);
        }
        public async Task RestoreTenant(int tenantId)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant);
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
            }
            var tenant = await TenantManager.FindByIdAsync(tenantId);

            tenant.IsDeleted = false;

            await TenantManager.UpdateAsync(tenant);
        }
    }
}
using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Cinotam.AbpModuleZero.Authorization
{
    public class AbpModuleZeroAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Common permissions
            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ??
                        context.CreatePermission(PermissionNames.Pages, L("Pages"));

            var organizationUnits = pages.CreateChildPermission(PermissionNames.PagesSysAdminOrgUnit, L("OrganizationUnits"));

            organizationUnits.CreateChildPermission(PermissionNames.PagesSysAdminOrgUnitEdit, L("OrganizationUnitsEdit"));
            organizationUnits.CreateChildPermission(PermissionNames.PagesSysAdminOrgUnitDelete, L("OrganizationUnitsDelete"));
            organizationUnits.CreateChildPermission(PermissionNames.PagesSysAdminOrgUnitCreate, L("OrganizationUnitsCreate"));
            organizationUnits.CreateChildPermission(PermissionNames.PagesSysAdminOrgUnitAddUser, L("OrganizationUnitsAddUser"));
            organizationUnits.CreateChildPermission(PermissionNames.PagesSysAdminOrgUnitRemoveUser, L("OrganizationUnitsRemoveUser"));

            //Configuration
            var configuration = pages.CreateChildPermission(PermissionNames.PagesSysAdminConfiguration, L("PagesSysAdminConfiguration"));
            //Dashboard
            var dashboard = pages.CreateChildPermission(PermissionNames.PagesDashboard, L("PagesSysAdminDashBoard"));
            //Users
            var users = pages.CreateChildPermission(PermissionNames.PagesSysAdminUsers, L("Users"));
            users.CreateChildPermission(PermissionNames.PagesSysAdminUsersCreate, L("CreateUsers"));
            users.CreateChildPermission(PermissionNames.PagesSysAdminUsersEdit, L("EditUsers"));
            users.CreateChildPermission(PermissionNames.PagesSysAdminUsersDelete, L("DeleteUsers"));

            //Languages
            var languages = pages.CreateChildPermission(PermissionNames.PagesSysAdminLanguages, L("PagesSysAdminLanguages"));
            languages.CreateChildPermission(PermissionNames.PagesSysAdminLanguagesCreate, L("PagesSysAdminLanguagesCreate"));
            languages.CreateChildPermission(PermissionNames.PagesSysAdminLanguagesDelete, L("PagesSysAdminLanguagesDelete"));
            languages.CreateChildPermission(PermissionNames.PagesSysAdminLanguagesChangeTexts, L("PagesSysAdminLanguagesChangeTexts"));

            //Permissions
            var permissions = pages.CreateChildPermission(PermissionNames.PagesSysAdminPermissions, L("PagesSysAdminPermissions"));

            //Roles
            var roles = pages.CreateChildPermission(PermissionNames.PagesSysAdminRoles, L("PagesSysAdminRoles"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesCreate, L("PagesSysAdminRolesCreate"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesEdit, L("PagesSysAdminRolesEdit"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesDelete, L("PagesSysAdminRolesDelete"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesAssign, L("PagesSysAdminRolesAssignPermissions"));

            //AuditLogs
            var auditLogs = pages.CreateChildPermission(PermissionNames.AuditLogs, L("PagesSysAuditLogs"));
            //Host permissions
            var tenants = pages.CreateChildPermission(PermissionNames.PagesTenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.PagesTenantsCreate, L("TenantsCreate"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.PagesTenantsEdit, L("TenantsEdit"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.PagesTenantsDelete, L("TenantsDelete"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.PagesTenantsAssignEdition, L("TenantsAssignEdition"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(PermissionNames.PagesTenantsAssignFeatures, L("TenantsAssignFeature"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

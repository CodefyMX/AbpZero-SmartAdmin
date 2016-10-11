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
            pages.CreateChildPermission(PermissionNames.PagesSysAdminConfiguration, L("PagesSysAdminConfiguration"));
            pages.CreateChildPermission(PermissionNames.PagesDashboard, L("PagesSysAdminDashBoard"));

            var users = pages.CreateChildPermission(PermissionNames.PagesSysAdminUsers, L("Users"));
            users.CreateChildPermission(PermissionNames.PagesSysAdminUsersCreate, L("CreateUsers"));
            users.CreateChildPermission(PermissionNames.PagesSysAdminUsersEdit, L("EditUsers"));
            users.CreateChildPermission(PermissionNames.PagesSysAdminUsersDelete, L("DeleteUsers"));

            var languages = pages.CreateChildPermission(PermissionNames.PagesSysAdminLanguages, L("PagesSysAdminLanguages"));
            languages.CreateChildPermission(PermissionNames.PagesSysAdminLanguagesCreate, L("PagesSysAdminLanguagesCreate"));
            languages.CreateChildPermission(PermissionNames.PagesSysAdminLanguagesDelete, L("PagesSysAdminLanguagesDelete"));
            languages.CreateChildPermission(PermissionNames.PagesSysAdminLanguagesChangeTexts, L("PagesSysAdminLanguagesChangeTexts"));

            pages.CreateChildPermission(PermissionNames.PagesSysAdminPermissions, L("PagesSysAdminPermissions"));

            var roles = pages.CreateChildPermission(PermissionNames.PagesSysAdminRoles, L("PagesSysAdminRoles"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesCreate, L("PagesSysAdminRolesCreate"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesEdit, L("PagesSysAdminRolesEdit"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesDelete, L("PagesSysAdminRolesDelete"));
            roles.CreateChildPermission(PermissionNames.PagesSysAdminRolesAssign, L("PagesSysAdminRolesAssignPermissions"));

            pages.CreateChildPermission(PermissionNames.AuditLogs, L("PagesSysAuditLogs"));
            //Host permissions
            var tenants = pages.CreateChildPermission(PermissionNames.PagesTenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

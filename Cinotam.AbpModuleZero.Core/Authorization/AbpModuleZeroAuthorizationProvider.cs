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


            var users = pages.CreateChildPermission(PermissionNames.PagesSysAdminUsers, L("Users"));

            pages.CreateChildPermission(PermissionNames.PagesSysAdminConfiguration, L("PagesSysAdminConfiguration"));

            pages.CreateChildPermission(PermissionNames.PagesSysAdminDashBoard, L("PagesSysAdminDashBoard"));

            pages.CreateChildPermission(PermissionNames.PagesSysAdminLanguages, L("PagesSysAdminLanguages"));

            pages.CreateChildPermission(PermissionNames.PagesSysAdminPermissions, L("PagesSysAdminPermissions"));

            pages.CreateChildPermission(PermissionNames.PagesSysAdminRoles, L("PagesSysAdminRoles"));
            //Host permissions
            var tenants = pages.CreateChildPermission(PermissionNames.PagesTenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

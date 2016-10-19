using Abp.Application.Navigation;
using Abp.Localization;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Authorization;

namespace Cinotam.ModuleZero.AppModule
{
    public class ModuleZeroNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.Menus.Add("ModuleZeroMenu", new MenuDefinition("ModuleZeroMenu", L("SysAdminMenu"))
                .AddItem(
                new MenuItemDefinition(
                        "Administration",
                        L("Administration")
                    ).AddItem(new MenuItemDefinition(
                    "Dashboard",
                    L("Dashboard"),
                    url: "/SysAdmin/Dashboard/",
                    requiredPermissionName: PermissionNames.PagesDashboard
                    )).AddItem(new MenuItemDefinition(
                    "OrganizationUnits",
                    L("OrganizationUnits"),
                    url: "/OrganizationUnits/OrganizationUnitsList/",
                    requiredPermissionName: PermissionNames.PagesSysAdminOrgUnit
                    )).AddItem(new MenuItemDefinition(
                    "Users",
                    L("Users"),
                    url: "/SysAdmin/Users/UsersList/",
                    requiredPermissionName: PermissionNames.PagesSysAdminUsers
                    )).AddItem(new MenuItemDefinition(
                    "Roles",
                    L("Roles"),
                    url: "/SysAdmin/Roles/RolesList/",
                    requiredPermissionName: PermissionNames.PagesSysAdminRoles
                    )).AddItem(new MenuItemDefinition(
                    "Languages",
                    L("Languages"),
                    url: "/SysAdmin/Languages/LanguagesList/",
                    requiredPermissionName: PermissionNames.PagesSysAdminLanguages
                    )).AddItem(new MenuItemDefinition(
                    "Configuration",
                    L("Configuration"),
                    url: "/SysAdmin/Configuration/Configurations/",
                    requiredPermissionName: PermissionNames.PagesSysAdminConfiguration
                    )).AddItem(new MenuItemDefinition(
                    "AuditLogs",
                    L("AuditLogs"),
                    url: "/SysAdmin/AuditLogs/AuditLogsList/",
                    requiredPermissionName: PermissionNames.AuditLogs
                    )).AddItem(new MenuItemDefinition(
                    "Editions",
                    L("Editions"),
                    url: "/SysAdmin/Editions/EditionList/",
                    requiredPermissionName: PermissionNames.PagesTenantsEdit
                    )).AddItem(new MenuItemDefinition(
                    "Tenants",
                    L("Tenants"),
                    url: "/SysAdmin/Tenants/TenantsList/",
                    requiredPermissionName: PermissionNames.PagesTenants
                    ))
                )
            );

        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

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
                        L("Administration"),
                        icon: "fa fa-lg fa-fw fa-gear",
                        customData: new { ActionName = "Index", ControllerName = "Dashboard" }
                    )
                    .AddItem(new MenuItemDefinition(
                    "Dashboard",
                    L("Dashboard"),
                    url: "/SysAdmin/Dashboard/",
                     customData: new { ActionName = "Index", ControllerName = "Dashboard" },
                    requiredPermissionName: PermissionNames.PagesDashboard
                    )).AddItem(new MenuItemDefinition(
                    "OrganizationUnits",
                    L("OrganizationUnits"),
                    url: "/SysAdmin/OrganizationUnits/OrganizationUnitsList/",
                     customData: new { ActionName = "OrganizationUnitsList", ControllerName = "OrganizationUnits" },
                    requiredPermissionName: PermissionNames.PagesSysAdminOrgUnit
                    )).AddItem(new MenuItemDefinition(
                    "Users",
                    L("Users"),
                    url: "/SysAdmin/Users/UsersList/",
                     customData: new { ActionName = "UsersList", ControllerName = "Users" },
                    requiredPermissionName: PermissionNames.PagesSysAdminUsers
                    )).AddItem(new MenuItemDefinition(
                    "Roles",
                    L("Roles"),
                    url: "/SysAdmin/Roles/RolesList/",
                     customData: new { ActionName = "RolesList", ControllerName = "Roles" },
                    requiredPermissionName: PermissionNames.PagesSysAdminRoles
                    )).AddItem(new MenuItemDefinition(
                    "Languages",
                    L("Languages"),
                    url: "/SysAdmin/Languages/LanguagesList/",
                     customData: new { ActionName = "LanguagesList", ControllerName = "Languages" },
                    requiredPermissionName: PermissionNames.PagesSysAdminLanguages
                    )).AddItem(new MenuItemDefinition(
                    "Configuration",
                    L("Configuration"),
                    url: "/SysAdmin/Configuration/Configurations/",
                     customData: new { ActionName = "Configurations", ControllerName = "Configuration" },
                    requiredPermissionName: PermissionNames.PagesSysAdminConfiguration
                    )).AddItem(new MenuItemDefinition(
                    "AuditLogs",
                    L("AuditLogs"),
                    url: "/SysAdmin/AuditLogs/AuditLogsList/",
                     customData: new { ActionName = "AuditLogsList", ControllerName = "AuditLogs" },
                    requiredPermissionName: PermissionNames.AuditLogs
                    )).AddItem(new MenuItemDefinition(
                    "Editions",
                    L("Editions"),
                    url: "/SysAdmin/Editions/EditionList/",
                     customData: new { ActionName = "EditionList", ControllerName = "Editions" },
                    requiredPermissionName: PermissionNames.PagesTenantsEdit
                    )).AddItem(new MenuItemDefinition(
                    "Tenants",
                    L("Tenants"),
                    url: "/SysAdmin/Tenants/TenantsList/",
                    requiredPermissionName: PermissionNames.PagesTenants,
                    customData: new { ActionName = "TenantsList", ControllerName = "Tenants" }
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

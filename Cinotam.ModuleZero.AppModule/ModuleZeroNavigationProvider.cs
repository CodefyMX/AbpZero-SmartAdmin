using Abp.Application.Navigation;
using Abp.Localization;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.AngularHelpers;

namespace Cinotam.ModuleZero.AppModule
{
    public class ModuleZeroNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            const string adminFolder = "/App/SysAdmin/Main/modules/web/";

            BuildAdminMenu(context, adminFolder);
        }

        private void BuildAdminMenu(INavigationProviderContext context, string appMainModulesWeb)
        {
            context.Manager.Menus.Add("ModuleZeroMenu", new MenuDefinition("ModuleZeroMenu", L("SysAdminMenu"))
                .AddItem(
                new MenuItemDefinition(
                        "Administration",
                        L("Administration"),
                        icon: "fa fa-lg fa-fw fa-gear",
                        customData: new
                        {
                            //Required for custom menu resolver
                            ActionName = "Index",
                            ControllerName = "Dashboard",
                            //Required for angular spa admin page
                            AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesDashboard, "Dashboard", "dashboard", appMainModulesWeb + "dashboard/index.cshtml", false)
                        }
                    )


                    .AddItem(new MenuItemDefinition(
                    "Dashboard",
                    L("Dashboard"),
                    url: "/SysAdmin/Dashboard/",
                    customData: new
                    {
                        ActionName = "Index",
                        ControllerName = "Dashboard",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesDashboard, "Dashboard", "/dashboard", appMainModulesWeb + "dashboard/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.PagesDashboard
                    ))

                    .AddItem(new MenuItemDefinition(
                    "OrganizationUnits",
                    L("OrganizationUnits"),
                    url: "/SysAdmin/OrganizationUnits/OrganizationUnitsList/",
                    customData: new
                    {
                        ActionName = "OrganizationUnitsList",
                        ControllerName = "OrganizationUnits",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesSysAdminOrgUnit, "OrganizationUnits", "/organizationUnits", "organizationUnits/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.PagesSysAdminOrgUnit
                    ))

                    .AddItem(new MenuItemDefinition(
                    "Users",
                    L("Users"),
                    url: "/SysAdmin/Users/UsersList/",
                    customData: new
                    {
                        ActionName = "UsersList",
                        ControllerName = "Users",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesSysAdminUsers, "Users", "/users", "users/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.PagesSysAdminUsers
                    ))

                    .AddItem(new MenuItemDefinition(
                    "Roles",
                    L("Roles"),
                    url: "/SysAdmin/Roles/RolesList/",
                    customData: new
                    {
                        ActionName = "RolesList",
                        ControllerName = "Roles",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesSysAdminRoles, "Roles", "/roles", "roles/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.PagesSysAdminRoles
                    ))

                    .AddItem(new MenuItemDefinition(
                    "Languages",
                    L("Languages"),
                    url: "/SysAdmin/Languages/LanguagesList/",
                    customData: new
                    {
                        ActionName = "LanguagesList",
                        ControllerName = "Languages",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesSysAdminLanguages, "Languages", "/languages", "languages/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.PagesSysAdminLanguages
                    ))

                    .AddItem(new MenuItemDefinition(
                    "Configuration",
                    L("Configuration"),
                    url: "/SysAdmin/Configuration/Configurations/",
                    customData: new
                    {
                        ActionName = "Configurations",
                        ControllerName = "Configuration",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesSysAdminConfiguration, "Configurations", "/configurations", "configurations/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.PagesSysAdminConfiguration
                    ))

                    .AddItem(new MenuItemDefinition(
                    "AuditLogs",
                    L("AuditLogs"),
                    url: "/SysAdmin/AuditLogs/AuditLogsList/",
                    customData: new
                    {
                        ActionName = "AuditLogsList",
                        ControllerName = "AuditLogs",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.AuditLogs, "AuditLogs", "/AuditLogs", "auditLogs/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.AuditLogs
                    ))
                    .AddItem(new MenuItemDefinition(
                    "Editions",
                    L("Editions"),
                    url: "/SysAdmin/Editions/EditionList/",
                    customData: new
                    {
                        ActionName = "EditionList",
                        ControllerName = "Editions",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesTenantsEdit, "Editions", "/Editions", "editions/index.cshtml", false)
                    },
                    requiredPermissionName: PermissionNames.PagesTenantsEdit
                    ))
                    .AddItem(new MenuItemDefinition(
                    "Tenants",
                    L("Tenants"),
                    url: "/SysAdmin/Tenants/TenantsList/",
                    requiredPermissionName: PermissionNames.PagesTenants,
                    customData: new
                    {
                        ActionName = "TenantsList",
                        ControllerName = "Tenants",
                        AngularMenu = new AngularCustomObj.AngularMenuItem(true, PermissionNames.PagesTenants, "Tenants", "/Tenants", "tenants/index.cshtml", false)
                    }
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

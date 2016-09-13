using Abp.Application.Navigation;
using Abp.Localization;
using Cinotam.AbpModuleZero.Authorization;

namespace Cinotam.AbpModuleZero.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See Views/Layout/_TopMenu.cshtml file to know how to render menu.
    /// </summary>
    public class AbpModuleZeroNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(new MenuItemDefinition(
                        "Admin",
                        L("Admin"),
                        url: "/SysAdmin/Dashboard",
                        icon: "fa fa-dashboard",
                        requiredPermissionName: PermissionNames.PagesDashboard,
                        requiresAuthentication: true
                    ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

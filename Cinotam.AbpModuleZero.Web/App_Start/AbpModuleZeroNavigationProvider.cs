using Abp.Application.Navigation;
using Abp.Localization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Tools.AngularHelpers;

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
                        url: "/Home/AppSelector",
                        icon: "fa fa-dashboard",
                        requiredPermissionName: PermissionNames.PagesDashboard,
                        requiresAuthentication: true,
                        customData: new
                        {
                            AngularMenu = new AngularCustomObj.AngularMenuItem(false, "", "", "", "", false)
                        }

                    ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

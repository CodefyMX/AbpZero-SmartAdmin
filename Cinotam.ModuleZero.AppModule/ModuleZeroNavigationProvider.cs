using Abp.Application.Navigation;
using Abp.Localization;
using Cinotam.AbpModuleZero;

namespace Cinotam.ModuleZero.AppModule
{
    public class ModuleZeroNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.Menus.Add("ModuleZeroMenu", new MenuDefinition("ModuleZeroMenu", L("SysAdminMenu"))
                .AddItem(new MenuItemDefinition(
                    "Users",
                    L("Users"),
                    url: "/Users"
                    ))
                );
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

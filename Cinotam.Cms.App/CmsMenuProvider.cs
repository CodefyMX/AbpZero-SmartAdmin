using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Localization;
using Cinotam.AbpModuleZero;

namespace Cinotam.Cms.App
{
    public class CmsMenuProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.Menus.Add("Cms",
                new MenuDefinition(
                    name: "Cms",
                    displayName: L("Cms")
                    )
                .AddItem(
                    new MenuItemDefinition(
                        name: "CMSSystem",
                        displayName: L("CMSSystem"),
                        icon: "fa fa-lg fa-fw fa-beer",
                        featureDependency: new SimpleFeatureDependency(FeatureNames.Cms),
                        customData: new { ActionName = "MyPages", ControllerName = "Cms" },
                        requiresAuthentication: true
                    ).AddItem(
                    new MenuItemDefinition(
                        name: "CMSCategories",
                        displayName: L("CMSCategories"),
                        url: "/SysAdmin/PageCategories/MyCategories",
                         icon: "fa fa-folder-o",
                        featureDependency: new SimpleFeatureDependency(FeatureNames.Cms),
                        customData: new { ActionName = "MyCategories", ControllerName = "Categories" },
                        requiresAuthentication: true)
                    ).AddItem(
                    new MenuItemDefinition(
                        name: "CMSMenus",
                        displayName: L("CMSMenus"),
                        url: "/SysAdmin/Menu/MyMenus",
                         icon: "fa fa-bars",
                        featureDependency: new SimpleFeatureDependency(FeatureNames.Cms),
                        customData: new { ActionName = "MyMenus", ControllerName = "Menu" },
                        requiresAuthentication: true)
                    ).AddItem(
                    new MenuItemDefinition(
                        name: "CMSPages",
                        displayName: L("CMSPages"),
                        url: "/SysAdmin/Cms/MyPages",
                        icon: "fa fa-file-word-o",
                        featureDependency: new SimpleFeatureDependency(FeatureNames.Cms),
                        customData: new { ActionName = "MyPages", ControllerName = "Cms" },
                        requiresAuthentication: true)
                    ).AddItem(
                    new MenuItemDefinition(
                        name: "CMSTemplates",
                        displayName: L("CMSTemplates"),
                        url: "/SysAdmin/Templates/MyTemplates",
                        icon: "fa fa-file-code-o",
                        featureDependency: new SimpleFeatureDependency(FeatureNames.Cms),
                        customData: new { ActionName = "MyTemplates", ControllerName = "Templates" },
                        requiresAuthentication: true)
                    )


                    )
                );
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

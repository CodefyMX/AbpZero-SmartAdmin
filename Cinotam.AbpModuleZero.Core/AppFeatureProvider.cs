using Abp.Application.Features;
using Abp.Localization;
using Abp.UI.Inputs;

namespace Cinotam.AbpModuleZero
{
    public class AppFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {


            var cms = context.Create(FeatureNames.Cms, "false", L("Cms"), scope: FeatureScopes.All, inputType: new CheckboxInputType());

            var basicReporting = context.Create("BasicReporting",
               "false",
               L("basicReporting"),
               L("BasicReportingDescription"),
               FeatureScopes.Edition, new CheckboxInputType());
            var advancedReporting = context.Create("AdvancedReporting",
                "false",
                L("AdvancedReporting"),
                L("AdvancedReportingDescription"),
                FeatureScopes.Edition, new CheckboxInputType());

            advancedReporting.CreateChildFeature("PrintData",
                defaultValue: "false",
                scope: FeatureScopes.Edition, inputType: new CheckboxInputType());
            advancedReporting.CreateChildFeature("PrintDataMaximum",
                defaultValue: "false",
                scope: FeatureScopes.Edition, inputType: new CheckboxInputType());

            advancedReporting.CreateChildFeature("MaxReportsPerMonth",
                defaultValue: "100",
                scope: FeatureScopes.Edition, inputType: new SingleLineStringInputType());

            advancedReporting.CreateChildFeature("HighCharts",
                "false",
                L("HighCharts"),
                L("HighChartsDescription"),
                FeatureScopes.Edition, new CheckboxInputType());
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

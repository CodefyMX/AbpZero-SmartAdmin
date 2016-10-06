using Abp.Application.Features;
using Abp.Localization;
using Abp.UI.Inputs;

namespace Cinotam.AbpModuleZero
{
    public class AppFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            var cmsEnabled = context.Create("CmsEnabled",
                "false", L("CmsEnabled"),
                L("CmsDescription"),
                inputType: new CheckboxInputType()
            );
            cmsEnabled.CreateChildFeature("MaxPageNumber",
                defaultValue: "10",
                displayName: L("MaxPageNumber"),
                description: L("MaxPageNameDescription"),
                inputType: new SingleLineStringInputType());
            var commentBox = cmsEnabled.CreateChildFeature("CommentBox", defaultValue: "true", inputType: new CheckboxInputType());
            commentBox.CreateChildFeature("AdminComentBox", defaultValue: "false", inputType: new CheckboxInputType());
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpModuleZeroConsts.LocalizationSourceName);
        }
    }
}

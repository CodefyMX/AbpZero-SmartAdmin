using Abp.Web.Mvc.Views;

namespace Cinotam.AbpModuleZero.Web.Views
{
    public abstract class AbpModuleZeroWebViewPageBase : AbpModuleZeroWebViewPageBase<dynamic>
    {

    }

    public abstract class AbpModuleZeroWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected AbpModuleZeroWebViewPageBase()
        {
            LocalizationSourceName = AbpModuleZeroConsts.LocalizationSourceName;

        }
    }
}
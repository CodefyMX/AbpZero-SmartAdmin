using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using Cinotam.AbpModuleZero.TenantHelpers.TenantHelperAppServiceBase;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.MultiTenancy.MultiTenancyHelper;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class AbpModuleZeroControllerBase : AbpController
    {
        public IMultiTenancyHelper MultiTenancyHelper { get; set; }
        public ITenantHelperService TenantAppService { get; set; }
        private const string TenancyKey = "CurrentTenant";
        protected AbpModuleZeroControllerBase()
        {
            LocalizationSourceName = AbpModuleZeroConsts.LocalizationSourceName;
        }

        /// <summary>
        /// Build the model for the datatables.js request
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="propToSearch">Prop used to filter data if empty search in all props of the object</param>
        /// <param name="reflectedProps">Columns of the table, they need to be in order</param>
        protected void ProccessQueryData(RequestModel<object> requestModel, string propToSearch, string[] reflectedProps)
        {
            if (
                Request.QueryString["order[0][column]"] != null)
            {
                requestModel.PropSort = int.Parse(Request.QueryString["order[0][column]"]);
            }
            if (Request.QueryString["order[0][dir]"] != null)
            {
                requestModel.PropOrd = Request.QueryString["order[0][dir]"];
            }

            if (!string.IsNullOrEmpty(propToSearch)) requestModel.PropToSearch = propToSearch;
            try
            {
                requestModel.PropToSort = reflectedProps[requestModel.PropSort];
            }
            catch
            {
                throw new Exception("Rango de propiedades invalido.");
            }
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected void CreateCookie(string name, string value)
        {
            if (HttpContext.Session != null) HttpContext.Session[name] = value;
        }

        protected object GetCookieValue(string name)
        {
            return HttpContext.Session?[name];
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentTenant = string.Empty;
            if (Request.Url != null) currentTenant = MultiTenancyHelper.GetCurrentTenancyName(Request.Url.AbsoluteUri);

            Session[TenancyKey] = currentTenant;

            base.OnActionExecuting(filterContext);
        }

        public bool UrlHasAValidTenancyName => TenantAppService.IsAValidTenancyName(GetTenancyNameFromSession);

        public string GetTenancyNameFromSession => Session[TenancyKey]?.ToString() ?? string.Empty;
    }
}
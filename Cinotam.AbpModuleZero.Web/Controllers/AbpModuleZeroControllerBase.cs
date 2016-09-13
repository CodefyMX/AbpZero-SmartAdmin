using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Microsoft.AspNet.Identity;
using System;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class AbpModuleZeroControllerBase : AbpController
    {
        protected AbpModuleZeroControllerBase()
        {
            LocalizationSourceName = AbpModuleZeroConsts.LocalizationSourceName;
        }

        //protected override void ExecuteCore()
        //{
        //    var lang = GetCurrentCulture();
        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang, false);

        //    // set the lang value into route data
        //    RouteData.Values["lang"] = lang;

        //    // save the location into cookie
        //    var cookie = new HttpCookie("DPClick.CurrentUICulture",
        //        Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName)
        //    {
        //        Expires = DateTime.Now.AddYears(1)
        //    };

        //    HttpContext.Response.SetCookie(cookie);
        //    base.ExecuteCore();
        //}

        //private string GetCurrentCulture()
        //{

        //    string lang;

        //    // set the culture from the route data (url)

        //    if (RouteData.Values["lang"] != null &&
        //       !string.IsNullOrWhiteSpace(RouteData.Values["lang"].ToString()))
        //    {
        //        lang = RouteData.Values["lang"].ToString();
        //        return lang;
        //    }
        //    // load the culture info from the cookie
        //    HttpCookie cookie = HttpContext.Request.Cookies["DPClick.CurrentUICulture"];
        //    if (cookie != null)
        //    {
        //        // set the culture by the cookie content
        //        lang = cookie.Value;
        //        return lang;

        //    }
        //    // set the culture by the location if not speicified
        //    if (HttpContext.Request.UserLanguages != null)
        //    {
        //        lang = HttpContext.Request.UserLanguages[0];

        //        return lang;
        //    }


        //    //English is default
        //    return "en";

        //}


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
    }
}
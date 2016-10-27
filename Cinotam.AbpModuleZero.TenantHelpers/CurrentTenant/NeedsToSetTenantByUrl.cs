using System;
using System.Web;

namespace Cinotam.AbpModuleZero.TenantHelpers.CurrentTenant
{
    /// <summary>
    /// 
    /// (Not working) its only a possible approach
    /// 
    /// Sets the current tenant by url
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]

    public class NeedsToSetTenantByUrl : Attribute
    {
        public NeedsToSetTenantByUrl()
        {
            var session = HttpContext.Current.Session;

        }
    }
}

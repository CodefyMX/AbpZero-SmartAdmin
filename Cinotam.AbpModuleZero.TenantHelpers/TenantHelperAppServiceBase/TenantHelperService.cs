using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Cinotam.AbpModuleZero.MultiTenancy;
using System;
using System.Web;

namespace Cinotam.AbpModuleZero.TenantHelpers.TenantHelperAppServiceBase
{
    public class TenantHelperService : DomainService, ITenantHelperService
    {

        private const string TenancyKey = "CurrentTenant";
        private readonly IRepository<Tenant> _tenantRepository;
        public IAbpSession AbpSession { get; set; }
        public TenantHelperService(IRepository<Tenant> tenantRepository)
        {
            _tenantRepository = tenantRepository;
            AbpSession = NullAbpSession.Instance;
        }

        public void SetCurrentTenantFromUrl()
        {

            if (CurrentUnitOfWork == null)
            {
                using (UnitOfWorkManager.Begin())
                {
                    SetTenantId();
                }
            }
            else
            {
                SetTenantId();
            }
        }

        private void SetTenantId()
        {
            //Check if there is a active session
            if (AbpSession.TenantId.HasValue) return; //We dont need to switch the tenant

            var tenantName = GetSessionKey(TenancyKey);

            var tenant = _tenantRepository.FirstOrDefault(a => a.TenancyName.ToUpper() == tenantName.ToUpper());

            //If there is no tenant we simply ignore the request and work as default.
            //This means that there is not a valid tenant in the url 
            if (tenant == null) return;


            CurrentUnitOfWork.SetTenantId(tenant.Id);

        }

        public bool IsAValidTenancyName(string tenancyName)
        {
            var tenant = _tenantRepository.FirstOrDefault(a => a.TenancyName.ToUpper() == tenancyName.ToUpper());
            if (tenant == null) return false;
            return true;
        }
        /// <summary>
        /// Gets the current tenancy name from the session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetSessionKey(string key)
        {
            var result = string.Empty;

            try
            {
                result = HttpContext.Current.Session[key].ToString();
            }
            catch (Exception)
            {
                return result;
            }

            return result;
        }

    }
}

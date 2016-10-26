using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Cinotam.AbpModuleZero.MultiTenancy;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy.MultiTenancyHelper
{
    public class MultiTenancyHelper :DomainService, IMultiTenancyHelper
    {
        private IRepository<Tenant> _tenantRepository;

        public MultiTenancyHelper(IRepository<Tenant> tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public string SetCurrentTenancy(string absoluteUrl)
        {
            var tenancyName = GetTenancyNameByUrl(absoluteUrl);
            //At this point we have the correct tenancy name
            //The problem is, how to set the current tenant in every app service
            //As i understand the only way to change the current tenancy id is inside a CurrentUnitOfWork using
            //And the tenantId will be automatically restored after the current executing block...

            /* 
            int tenantId;
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                tenantId = _tenantRepository.FirstOrDefault(a => a.Name == tenancyName).Id; 
            }
            CurrentUnitOfWork.SetTenantId(tenantId); 
            */
            return tenancyName;
        }
        private string GetTenancyNameByUrl(string absoluteUri)
        {
            if (absoluteUri.IsNullOrEmpty()) return string.Empty;

            if (absoluteUri.StartsWith("www"))
            {
               //Little cheat
                absoluteUri = "http://" + absoluteUri;
            }
            var tenancyName = absoluteUri.Split(".").First(a => !a.Contains("http") || !a.Contains("www"));
            tenancyName = tenancyName.Split("//").Last();
            return tenancyName;
        }
    }
}

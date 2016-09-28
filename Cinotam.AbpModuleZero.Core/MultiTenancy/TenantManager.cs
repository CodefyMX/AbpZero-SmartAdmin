﻿using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Cinotam.AbpModuleZero.Editions;
using Cinotam.AbpModuleZero.Users;

namespace Cinotam.AbpModuleZero.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore
            )
            : base(
                tenantRepository,
                tenantFeatureRepository,
                editionManager,
                featureValueStore
            )
        {
        }
    }
}
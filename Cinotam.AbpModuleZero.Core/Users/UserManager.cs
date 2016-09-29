using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Cinotam.AbpModuleZero.Authorization.Roles;

namespace Cinotam.AbpModuleZero.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        public UserManager(
            UserStore store,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            IdentityEmailMessageService identityEmailMessageService,
            IUserTokenProviderAccessor userTokenProviderAccessor
            )
            : base(
                store,
                roleManager,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                localizationManager,
                identityEmailMessageService,
                settingManager,
                userTokenProviderAccessor
                )
        {
        }
    }
}
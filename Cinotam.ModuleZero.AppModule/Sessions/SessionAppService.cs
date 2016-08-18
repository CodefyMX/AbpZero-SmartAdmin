using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Cinotam.ModuleZero.AppModule.Sessions.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Sessions
{
    [AbpAuthorize]
    public class SessionAppService : CinotamModuleZeroAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                User = (await GetCurrentUserAsync()).MapTo<UserLoginInfoDto>()
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            }

            return output;
        }
    }
}
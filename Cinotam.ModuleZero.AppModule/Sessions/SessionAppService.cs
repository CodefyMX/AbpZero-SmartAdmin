using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Chat;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.Sessions.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Sessions
{
    [AbpAuthorize]
    public class SessionAppService : CinotamModuleZeroAppServiceBase, ISessionAppService
    {
        private readonly IChatManager _chatManager;

        public SessionAppService(IChatManager chatManager)
        {
            _chatManager = chatManager;
        }

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

        public async Task<List<ChatLoginInformation>> GetCurrentLoginInformationsLs()
        {
            var output = new List<ChatLoginInformation>();
            var currentUserId = AbpSession.UserId;
            var users = UserManager.Users.Where(a => a.Id != currentUserId.Value).ToList();

            Tenant tenant = null;
            foreach (var user in users)
            {
                if (user.TenantId.HasValue)
                {

                    tenant = (await TenantManager.GetByIdAsync(user.TenantId.Value));

                }
                output.Add(new ChatLoginInformation()
                {
                    User = user.MapTo<UserLoginInfoDto>(),
                    ConversationId = await GetConversationId(user),
                    Tenant = tenant != null ? tenant.MapTo<TenantLoginInfoDto>() : new TenantLoginInfoDto() { Name = "", TenancyName = "" }
                });

            }
            return output;

        }

        private async Task<int?> GetConversationId(User user)
        {

            var currentUser = await GetCurrentUserAsync();
            var conversation = await _chatManager.GetConversation(currentUser, user, AbpSession.TenantId);

            return conversation?.Id;
        }
    }
}
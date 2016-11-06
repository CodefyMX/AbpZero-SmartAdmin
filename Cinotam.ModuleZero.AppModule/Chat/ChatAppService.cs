using Abp.AutoMapper;
using Abp.UI;
using Cinotam.AbpModuleZero.Chat;
using Cinotam.ModuleZero.AppModule.Chat.Dto;
using Cinotam.ModuleZero.Notifications.Chat.Outputs;
using Cinotam.ModuleZero.Notifications.Chat.Sender;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Chat
{
    public class ChatAppService : CinotamModuleZeroAppServiceBase, IChatAppService
    {
        private readonly IChatManager _chatManager;
        private readonly IChatMessageSender _chatMessageSender;
        public ChatAppService(IChatManager chatManager, IChatMessageSender chatMessageSender)
        {
            _chatManager = chatManager;
            _chatMessageSender = chatMessageSender;
        }

        public async Task SendMessage(SendMessageInput input)
        {
            if (!input.From.HasValue || !input.To.HasValue) throw new UserFriendlyException(L("NoUserDetected"));

            var from = await UserManager.GetUserByIdAsync(input.From.Value);
            var to = await UserManager.GetUserByIdAsync(input.To.Value);
            var id = await _chatManager.CreateConversation(from, to, AbpSession.TenantId);

            var conversation = _chatManager.Conversations.FirstOrDefault(a => a.Id == id);

            var msId = await _chatManager.AddMessage(conversation, from, input.Message);

            if (msId != 0)
            {
                await _chatMessageSender.PublishMessage(
                    new ChatData(from.MapTo<UserOutput>(),
                    to.MapTo<UserOutput>(),
                    id,
                    input.Message));
            }
        }

        public async Task<int> CreateConversation(SendMessageInput input)
        {
            if (!input.From.HasValue || !input.To.HasValue) throw new UserFriendlyException(L("NoUserDetected"));
            var from = await UserManager.GetUserByIdAsync(input.From.Value);
            var to = await UserManager.GetUserByIdAsync(input.To.Value);
            var id = await _chatManager.CreateConversation(from, to, AbpSession.TenantId);
            return id;
        }
        public Task<ConversationOutput> LoadConversation(ConversationRequestInput input)
        {
            throw new NotImplementedException();
        }
    }
}


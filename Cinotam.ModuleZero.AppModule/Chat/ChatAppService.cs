using Cinotam.ModuleZero.AppModule.Chat.Dto;
using System;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Chat
{
    public class ChatAppService : CinotamModuleZeroAppServiceBase, IChatAppService
    {
        public Task SendMessage(SendMessageInput input)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationOutput> LoadConversation(ConversationRequestInput input)
        {
            throw new NotImplementedException();
        }
    }
}

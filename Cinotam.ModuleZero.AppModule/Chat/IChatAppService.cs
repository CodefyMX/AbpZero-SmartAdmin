using Abp.Application.Services;
using Cinotam.ModuleZero.AppModule.Chat.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Chat
{
    public interface IChatAppService : IApplicationService
    {
        Task SendMessage(SendMessageInput input);
        Task<ConversationOutput> LoadConversation(ConversationRequestInput input);


    }
}

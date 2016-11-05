using Cinotam.AbpModuleZero.Chat.Entities;
using Cinotam.AbpModuleZero.Users;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.Chat
{
    public interface IChatManager
    {
        IQueryable<Conversation> Conversations { get; }

        Task<int> CreateConversation(User @from, User to, int? tenantId);

        Task<Conversation> GetConversation(User @from, User to, int? tenantId);
        Task<int> AddMessage(Conversation conversation, User user, string message);
    }
}

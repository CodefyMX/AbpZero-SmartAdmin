using Cinotam.AbpModuleZero.Users;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.Chat
{
    public interface IChatManager
    {
        IQueryable Conversation { get; }

        Task<int> CreateConversation(User @from, User to, int? tenantId);

        Task<IQueryable> GetConversation(User from, User to, int? tenantId);
    }
}

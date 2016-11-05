using Cinotam.ModuleZero.Notifications.Chat.Outputs;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.Chat.Sender
{
    public interface IChatMessageSender
    {
        Task PublishMessage(ChatData data);
    }
}

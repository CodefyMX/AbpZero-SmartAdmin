using Abp.Notifications;

namespace Cinotam.ModuleZero.Notifications.Chat.Outputs
{
    public class ChatData : NotificationData
    {
        public ChatData(ChatUserOutput from, ChatUserOutput to, string message)
        {
            To = to;
            From = from;
        }

        public ChatUserOutput To { get; private set; }

        public ChatUserOutput From { get; private set; }
    }
}

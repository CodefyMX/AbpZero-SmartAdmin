using Abp.Notifications;

namespace Cinotam.ModuleZero.Notifications.Chat.Outputs
{
    public class ChatData : NotificationData
    {
        public ChatData(UserOutput from, UserOutput to, int conversationId, string message)
        {
            To = to;
            From = from;
            ConversationId = conversationId;
            Message = message;
        }

        public string Message { get; set; }

        /// <summary>
        /// This works as a notification subscription id
        /// </summary>
        public int ConversationId { get; set; }

        public UserOutput To { get; private set; }

        public UserOutput From { get; private set; }
    }
}

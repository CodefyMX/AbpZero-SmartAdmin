using Abp;
using Abp.Domain.Services;
using Abp.Notifications;
using Cinotam.ModuleZero.Notifications.Chat.Outputs;
using System;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.Chat.Sender
{
    public class ChatMessageSender : DomainService, IChatMessageSender
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public ChatMessageSender(INotificationPublisher notificationPublisher,
            INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public async Task PublishMessage(ChatData data)
        {

            var conversationId = "ChatMessage";

            var userIndentifierFrom = new UserIdentifier(data.From.TenantId, data.From.Id);

            var userIndentifierTo = new UserIdentifier(data.To.TenantId, data.To.Id);

            UserIdentifier[] userIds = { userIndentifierTo, userIndentifierFrom };

            await SubscribeFromToUsers(conversationId, userIndentifierFrom, userIndentifierTo);

            try
            {
                await _notificationPublisher.PublishAsync(conversationId, data, userIds: userIds);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Subscribe both users to the conversation
        /// </summary>
        /// <param name="convId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private async Task SubscribeFromToUsers(string convId, UserIdentifier from, UserIdentifier to)
        {

            await _notificationSubscriptionManager.SubscribeAsync(from, convId);
            await _notificationSubscriptionManager.SubscribeAsync(to, convId);
        }
    }
}

using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Cinotam.AbpModuleZero.Chat.Entities;
using Cinotam.AbpModuleZero.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.Chat
{
    public class ChatManager : DomainService, IChatManager
    {
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IRepository<Message> _messagesRepository;
        public ChatManager(IRepository<Conversation> conversationRepository, IRepository<Message> messagesRepository)
        {
            _conversationRepository = conversationRepository;
            _messagesRepository = messagesRepository;
        }
        public IQueryable<Conversation> Conversations => _conversationRepository.GetAll();
        public async Task<int> CreateConversation(User from, User to, int? tenantId)
        {
            if (from.Id == to.Id) throw new InvalidOperationException(nameof(to));


            var conversationInDb = _conversationRepository.FirstOrDefault(a => a.From == from.Id && a.To == to.Id);

            if (conversationInDb == null)
            {
                var other = _conversationRepository.FirstOrDefault(a => a.To == from.Id && a.From == to.Id);
                if (other != null)
                    return other.Id;
            }
            else
            {
                return conversationInDb.Id;
            }
            var id = await _conversationRepository.InsertOrUpdateAndGetIdAsync(new Conversation()
            {
                From = from.Id,
                To = to.Id,
                TenantId = tenantId,
            });
            return id;
        }

        public async Task<int> AddMessage(Conversation conversation, User user, string message)
        {
            var id = await _messagesRepository.InsertOrUpdateAndGetIdAsync(new Message()
            {
                ConversationId = conversation.Id,
                MessageText = message,
                SenderId = user.Id
            });
            return id;
        }

        public async Task<Conversation> GetConversation(User from, User to, int? tenantId)
        {
            var conversationInDb = await _conversationRepository.FirstOrDefaultAsync(a => a.From == from.Id && a.To == to.Id);

            if (conversationInDb == null)
            {
                var other = await _conversationRepository.FirstOrDefaultAsync(a => a.To == from.Id && a.From == to.Id);
                return other;
            }
            return conversationInDb;
        }
    }
}

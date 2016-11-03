using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Cinotam.AbpModuleZero.Chat.Entities;
using Cinotam.AbpModuleZero.Users;
using System;
using System.Linq;
using System.Linq.Dynamic;
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
        public IQueryable Conversation => _conversationRepository.GetAll();
        public async Task<int> CreateConversation(User from, User to, int? tenantId)
        {
            if (from.Id == to.Id) throw new InvalidOperationException(nameof(to));


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
                Conversation = conversation,
                MessageText = message,
                SenderId = user.Id
            });
            return id;
        }

        public async Task<IQueryable> GetConversation(User @from, User to, int? tenantId)
        {
            var all = await Task.FromResult(from conv in _conversationRepository.GetAll()
                                            where conv.From == @from.Id
                                            && conv.To == to.Id
                                            select conv);
            return all;
        }
    }
}

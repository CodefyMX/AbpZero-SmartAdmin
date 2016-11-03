using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.AbpModuleZero.Chat.Entities
{
    public class Message : FullAuditedEntity
    {
        public int ConversationId { get; set; }
        [ForeignKey("ConversationId")]
        public virtual Conversation Conversation { get; set; }
        public long SenderId { get; set; }
        public string MessageText { get; set; }
    }
}

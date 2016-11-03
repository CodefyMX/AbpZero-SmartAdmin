using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Cinotam.AbpModuleZero.Chat.Entities
{
    public class Conversation : FullAuditedEntity, IMayHaveTenant
    {

        public long From { get; set; }

        public long To { get; set; }
        public int? TenantId { get; set; }
    }
}

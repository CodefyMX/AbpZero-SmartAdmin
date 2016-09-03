using Abp.Domain.Entities.Auditing;

namespace Cinotam.AbpModuleZero.SystemMails
{
    public class SystemMail : FullAuditedEntity
    {
        public string User { get; set; }
        public string MessageData { get; set; }
        public bool Sent { get; set; }
    }
}

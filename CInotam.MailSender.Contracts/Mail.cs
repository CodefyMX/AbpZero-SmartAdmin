using Abp.Domain.Entities.Auditing;

namespace CInotam.MailSender.Contracts
{
    public class Mail : FullAuditedEntity, IMail
    {
        public string From { get; set; }
        public string To { get; set; }

        public string Data { get; set; }
        public bool Sent { get; set; }

    }
}

using CInotam.MailSender.Contracts;

namespace Cinotam.MailSender.SendGrid.SendGrid.Outputs
{
    public class SendGridMessageResult : IMailServiceResult
    {
        public bool Success { get; set; }
        public string ErroMessage { get; set; }
        public bool MailSent { get; set; }
    }
}

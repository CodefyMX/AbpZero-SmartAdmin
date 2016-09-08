using CInotam.MailSender.Contracts;
using System.Net.Mail;

namespace Cinotam.MailSender.SendGrid.SendGrid.Inputs
{
    public class SendGridMessageInput : IMail
    {
        public MailMessage MailMessage { get; set; }
        public string HtmlView { get; set; }
        public string Body { get; set; }
        public string EncodeType { get; set; }
        public dynamic ExtraParams { get; set; }
        public bool Sent { get; set; }
    }
}

using CInotam.MailSender.Contracts;
using System.Net.Mail;

namespace Cinotam.ModuleZero.MailSender.CinotamMailSender.Inputs
{
    public class EmailSendInput : IMail
    {
        public EmailSendInput()
        {
            Sent = false;
        }
        public MailMessage MailMessage { get; set; }
        public string HtmlView { get; set; }
        public string Body { get; set; }
        public string EncodeType { get; set; }
        public dynamic ExtraParams { get; set; }
        public bool Sent { get; set; }
    }
}

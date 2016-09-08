using System.Net.Mail;

namespace CInotam.MailSender.Contracts
{
    public interface IMail
    {
        MailMessage MailMessage { get; set; }
        string HtmlView { get; set; }
        string Body { get; set; }
        string EncodeType { get; set; }
        dynamic ExtraParams { get; set; }
        bool Sent { get; set; }
    }
}

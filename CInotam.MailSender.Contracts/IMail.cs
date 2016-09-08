using System.Net.Mail;

namespace CInotam.MailSender.Contracts
{
    /// <summary>
    /// Serves as the main input for all the services
    /// </summary>
    public interface IMail
    {
        MailMessage MailMessage { get; set; }
        string HtmlView { get; set; }
        string Body { get; set; }
        string EncodeType { get; set; }
        /// <summary>
        /// The dinamyc property allows us to send any additional parameter to the 
        /// rest api
        /// </summary>
        dynamic ExtraParams { get; set; }
        bool Sent { get; set; }
    }
}

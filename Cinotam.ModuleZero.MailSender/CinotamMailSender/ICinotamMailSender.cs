using System.Threading.Tasks;
using Abp.Domain.Services;
using Cinotam.MailSender.CinotamMailSender.Inputs;
using Cinotam.MailSender.CinotamMailSender.Outputs;

namespace Cinotam.MailSender.CinotamMailSender
{
    public interface ICinotamMailSender : IDomainService
    {
        Task<EmailSentResult> SendMail(EmailSendInput input);
    }
}

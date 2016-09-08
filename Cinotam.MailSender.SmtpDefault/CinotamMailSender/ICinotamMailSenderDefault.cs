using Abp.Domain.Services;
using Cinotam.MailSender.SmtpDefault.CinotamMailSender.Outputs;
using CInotam.MailSender.Contracts;
using System.Threading.Tasks;

namespace Cinotam.MailSender.SmtpDefault.CinotamMailSender
{
    public interface ICinotamMailSenderDefault : IMailServiceProvider, IDomainService
    {
        Task<EmailSentResult> SendMail(IMail input);
    }
}

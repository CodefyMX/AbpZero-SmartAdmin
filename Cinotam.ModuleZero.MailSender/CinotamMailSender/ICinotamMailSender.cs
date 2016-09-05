using Abp.Domain.Services;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Inputs;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Outputs;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.MailSender.CinotamMailSender
{
    public interface ICinotamMailSender : IDomainService
    {
        Task<EmailSentResult> SendMail(EmailSendInput input);
    }
}

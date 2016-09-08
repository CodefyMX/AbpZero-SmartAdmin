using Abp.Domain.Services;
using Cinotam.MailSender.SendGrid.SendGrid.Outputs;
using CInotam.MailSender.Contracts;
using System.Threading.Tasks;

namespace Cinotam.MailSender.SendGrid.SendGrid
{
    public interface ISendGridService : IDomainService, IMailServiceProvider
    {
        Task<SendGridMessageResult> SendViaHttp(IMail mail);
    }
}

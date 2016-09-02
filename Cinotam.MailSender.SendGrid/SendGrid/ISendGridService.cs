using Abp.Domain.Services;
using Cinotam.MailSender.SendGrid.SendGrid.Inputs;
using Cinotam.MailSender.SendGrid.SendGrid.Outputs;
using System.Threading.Tasks;

namespace Cinotam.MailSender.SendGrid.SendGrid
{
    public interface ISendGridService : IDomainService
    {
        Task<SendGridMessageResult> SendViaHttp(SendGridMessageInput input);
    }
}

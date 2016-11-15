using Abp.Domain.Services;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorAuth.Contracts
{
    public interface IMessageSender : IIdentityMessageService, IDomainService
    {
        Task<SendMessageResult> SendSmsMessage(IdentityMessage message);
        Task<SendMessageResult> SendEmailMessage(IdentityMessage message);
        string ServiceName { get; }
    }
}

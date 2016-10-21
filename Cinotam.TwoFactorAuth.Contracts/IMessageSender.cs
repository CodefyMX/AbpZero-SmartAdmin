using Abp.Domain.Services;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorAuth.Contracts
{
    public interface IMessageSender : IIdentityMessageService, IDomainService
    {
        Task SendMessage(IdentityMessage message);
        string ServiceName { get; }
    }
}

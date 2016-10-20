using Abp.Domain.Services;
using Cinotam.TwoFactorAuth.Contracts;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorSender.Sender
{
    public interface ITwoFactorMessageService : IDomainService
    {
        Task SendMessage(CinotamAbpIdentityMessage message);
    }
}

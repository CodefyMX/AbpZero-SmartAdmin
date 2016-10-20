using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorAuth.Contracts
{
    public interface IMessageSender : IDomainService
    {
        Task SendMessage(CinotamAbpIdentityMessage message);
        string ServiceName { get; }
    }
}

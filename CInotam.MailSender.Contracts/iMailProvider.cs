using System.Threading.Tasks;

namespace CInotam.MailSender.Contracts
{
    public interface IMailProvider
    {
        Task<bool> SendMail(IMail mail);
    }
}

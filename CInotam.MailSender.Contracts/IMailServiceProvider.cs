using System.Threading.Tasks;

namespace CInotam.MailSender.Contracts
{
    public interface IMailServiceProvider
    {
        Task<IMailServiceResult> DeliverMail(IMail mail);
    }
}

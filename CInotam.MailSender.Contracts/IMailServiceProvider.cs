using System.Threading.Tasks;

namespace CInotam.MailSender.Contracts
{
    /// <summary>
    /// All mail services must implement this interface so they can be registered
    /// 
    /// </summary>
    public interface IMailServiceProvider
    {
        Task<IMailServiceResult> DeliverMail(IMail mail);
        bool IsSmtp { get; }
        bool IsHttp { get; }
    }
}

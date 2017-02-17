using Abp.Domain.Services;
using CInotam.MailSender.Contracts;

namespace Cinotam.MailSender.SmptGmail.CinotamMailSenderSmptGmal
{
    public interface ICinotamMailSenderGmail : IMailServiceProvider, IDomainService
    {

    }
}

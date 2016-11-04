using Abp.Domain.Services;
using RestApiHelpers.Contracts.Input;
using SendGrid;

namespace Cinotam.MailSender.SendGrid.Credentials
{
    public interface ISendGridCredentialsService : IDomainService
    {
        SendGridAPIClient GetInstance(RestApiCredentialsRequest credentialsRequest);
    }
}

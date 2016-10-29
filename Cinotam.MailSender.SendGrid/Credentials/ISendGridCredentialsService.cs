using Abp.Dependency;
using RestApiHelpers.Contracts.Input;
using SendGrid;

namespace Cinotam.MailSender.SendGrid.Credentials
{
    public interface ISendGridCredentialsService : ISingletonDependency
    {
        SendGridAPIClient GetInstance(RestApiCredentialsRequest credentialsRequest);
    }
}

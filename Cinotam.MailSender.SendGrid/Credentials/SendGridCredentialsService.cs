using RestApiHelpers.Contracts.Input;
using RestApiHelpers.Credentials;
using SendGrid;

namespace Cinotam.MailSender.SendGrid.Credentials
{
    /// <summary>
    /// Send grid only requires the api key
    /// </summary>
    public class SendGridCredentialsService : ISendGridCredentialsService
    {
        private readonly ICredentialsService _credentialsService;

        public SendGridCredentialsService(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        public SendGridAPIClient GetInstance(RestApiCredentialsRequest input)
        {
            var apiCredentials = _credentialsService.GetRestApiCredentials(input);
            var sg = new SendGridAPIClient(apiCredentials.Key);
            return sg;
        }
    }
}

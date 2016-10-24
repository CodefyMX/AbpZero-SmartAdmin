using Cinotam.TwoFactorAuth.Twilio.Credentials.Input;
using RestApiHelpers.Credentials;
using Twilio;

namespace Cinotam.TwoFactorAuth.Twilio.Credentials
{
    public class TwilioSenderCredentials : ITwilioSenderCredentials
    {
        private readonly ICredentialsService _credentialsService;

        public TwilioSenderCredentials(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        public TwilioRestClient GetClient(TwilioCredentials input)
        {
            var credentials = _credentialsService.GetRestApiCredentials(input);
            return new TwilioRestClient(credentials.Key, credentials.Secret);
        }
    }
}

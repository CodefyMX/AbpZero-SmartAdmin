using Cinotam.TwoFactorAuth.Twilio.Credentials.Input;
using System;
using Twilio;

namespace Cinotam.TwoFactorAuth.Twilio.Credentials
{
    public class TwilioSenderCredentials : ITwilioSenderCredentials
    {
        public TwilioRestClient GetClient(TwilioCredentials input)
        {
            var apiKey = Environment.GetEnvironmentVariable(input.ApiKeyVarName, input.EnvTarget);
            var apiSecret = Environment.GetEnvironmentVariable(input.ApiSecretVarName, input.EnvTarget);

            return new TwilioRestClient(apiKey, apiSecret);

        }
    }
}

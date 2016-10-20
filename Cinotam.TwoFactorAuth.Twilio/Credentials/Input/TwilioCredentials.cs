using System;

namespace Cinotam.TwoFactorAuth.Twilio.Credentials.Input
{
    public class TwilioCredentials
    {
        public string ApiKeyVarName { get; set; }
        public string ApiSecretVarName { get; set; }
        public EnvironmentVariableTarget EnvTarget { get; set; }
    }
}

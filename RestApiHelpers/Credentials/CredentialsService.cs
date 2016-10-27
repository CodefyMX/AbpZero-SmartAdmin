using RestApiHelpers.Contracts.Input;
using RestApiHelpers.Contracts.Output;
using RestApiHelpers.Enums;
using System;
using System.Configuration;

namespace RestApiHelpers.Credentials
{
    public class CredentialsService : ICredentialsService
    {
        public RestApiCredentials GetRestApiCredentials(RestApiCredentialsRequest input)
        {
            switch (input.Strategy)
            {
                case Strategy.WebConfig:

                    return GetFromWebConfig(input);

                case Strategy.EnvVar:
                    return GetFromEnv(input);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected RestApiCredentials GetFromEnv(RestApiCredentialsRequest input)
        {
            var apiKey = Environment.GetEnvironmentVariable(input.ApiKeyName, input.EnvTarget);
            var secret = Environment.GetEnvironmentVariable(input.SecretKeyName, input.EnvTarget);
            return new RestApiCredentials()
            {
                Key = apiKey,
                Secret = secret
            };
        }

        protected RestApiCredentials GetFromWebConfig(RestApiCredentialsRequest input)
        {
            var apiKey = ConfigurationManager.AppSettings[input.ApiKeyName];
            var secret = ConfigurationManager.AppSettings[input.SecretKeyName];
            return new RestApiCredentials()
            {
                Key = apiKey,
                Secret = secret
            };
        }
    }
}

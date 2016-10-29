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
            var apiKey = GetValueFromEnv(input.ApiKeyName, input.EnvTarget);
            var secret = GetValueFromEnv(input.SecretKeyName, input.EnvTarget);
            return new RestApiCredentials()
            {
                Key = apiKey,
                Secret = secret
            };
        }

        private string GetValueFromEnv(string key, EnvironmentVariableTarget envTarget)
        {
            try
            {
                var value = Environment.GetEnvironmentVariable(key, envTarget);
                return value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
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

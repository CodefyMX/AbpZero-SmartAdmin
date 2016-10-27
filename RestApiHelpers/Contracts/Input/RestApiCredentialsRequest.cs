using RestApiHelpers.Enums;
using System;

namespace RestApiHelpers.Contracts.Input
{
    public class RestApiCredentialsRequest
    {
        public RestApiCredentialsRequest(EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine)
        {
            EnvTarget = target;
        }
        public string SecretKeyName { get; set; }
        public string ApiKeyName { get; set; }
        public EnvironmentVariableTarget EnvTarget { get; set; }
        public Strategy Strategy { get; set; }
    }
}

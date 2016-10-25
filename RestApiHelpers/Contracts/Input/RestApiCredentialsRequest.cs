using RestApiHelpers.Enums;
using System;

namespace RestApiHelpers
{
    public class RestApiCredentialsRequest
    {
        public string SecretKeyName { get; set; }
        public string ApiKeyName { get; set; }
        public EnvironmentVariableTarget EnvTarget { get; set; }
        public Strategy Strategy { get; set; }
    }
}

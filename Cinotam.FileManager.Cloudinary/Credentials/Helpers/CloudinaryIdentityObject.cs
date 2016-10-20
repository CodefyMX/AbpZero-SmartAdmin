using System;

namespace Cinotam.FileManager.Cloudinary.Credentials.Helpers
{
    public class CloudinaryIdentityObject
    {
        public string ApiKeyVarName { get; set; }
        public string ApiSecretVarName { get; set; }
        public string CloudName { get; set; }
        public EnvironmentVariableTarget EnvTarget { get; set; }

    }
}

using Abp.Auditing;
using Cinotam.FileManager.Cloudinary.Credentials.Helpers;
using CloudinaryDotNet;
using System;

namespace Cinotam.FileManager.Cloudinary.Credentials
{
    public class Credentials : ICredentials
    {
        [DisableAuditing]
        public virtual CloudinaryDotNet.Cloudinary GetCloudinaryInstance(CloudinaryIdentityObject credentials)
        {


            var apiKey = Environment.GetEnvironmentVariable(credentials.ApiKeyVarName, credentials.EnvTarget);
            var secret = Environment.GetEnvironmentVariable(credentials.ApiSecretVarName, credentials.EnvTarget);
            var account = new Account()
            {
                ApiKey = apiKey,
                ApiSecret = secret,
                Cloud = credentials.CloudName
            };

            var instance = new CloudinaryDotNet.Cloudinary(account);
            return instance;
        }
    }
}

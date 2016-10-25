using Abp.Auditing;
using Cinotam.FileManager.Cloudinary.Credentials.Helpers;
using CloudinaryDotNet;
using RestApiHelpers.Credentials;
using System;

namespace Cinotam.FileManager.Cloudinary.Credentials
{
    public class Credentials : ICredentials
    {
        private readonly ICredentialsService _credentialsService;
        public Credentials(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }
        [DisableAuditing]
        public virtual CloudinaryDotNet.Cloudinary GetCloudinaryInstance(CloudinaryIdentityObject credentials)
        {
            var result = _credentialsService.GetRestApiCredentials(credentials);
            var account = new Account()
            {
                ApiKey = result.Key,
                ApiSecret = result.Secret,
                Cloud = credentials.CloudName
            };

            var instance = new CloudinaryDotNet.Cloudinary(account);
            return instance;
        }
    }
}

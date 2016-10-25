using RestApiHelpers;
using System;

namespace Cinotam.FileManager.Cloudinary.Credentials.Helpers
{
    public class CloudinaryIdentityObject : RestApiCredentialsRequest
    {
        public string CloudName { get; internal set; }
    }
}

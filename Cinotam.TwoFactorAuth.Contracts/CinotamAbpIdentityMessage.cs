using Microsoft.AspNet.Identity;

namespace Cinotam.TwoFactorAuth.Contracts
{
    public class CinotamAbpIdentityMessage : IdentityMessage
    {
        public string From { get; set; }
    }
}

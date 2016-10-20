using Abp.Domain.Services;
using Cinotam.TwoFactorAuth.Twilio.Credentials.Input;
using Twilio;

namespace Cinotam.TwoFactorAuth.Twilio.Credentials
{
    public interface ITwilioSenderCredentials : IDomainService
    {
        TwilioRestClient GetClient(TwilioCredentials input);
    }
}

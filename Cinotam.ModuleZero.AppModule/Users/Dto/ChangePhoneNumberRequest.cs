using Cinotam.ModuleZero.AppModule.Users.EnumHelpers;
using Cinotam.TwoFactorAuth.Contracts;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class ChangePhoneNumberRequest
    {
        public long UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryPhoneCode { get; set; }
        public string CountryCode { get; set; }
        public TwoFactorRequestResults ResultType { get; set; }

        public SendMessageResult SendMessageResult { get; set; }
    }
}

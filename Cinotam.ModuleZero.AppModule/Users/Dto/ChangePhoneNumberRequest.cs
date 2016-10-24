using Cinotam.ModuleZero.AppModule.Users.EnumHelpers;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class ChangePhoneNumberRequest
    {
        public long UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryPhoneCode { get; set; }
        public string CountryCode { get; set; }
        public TwoFactorRequestResults ResultType { get; set; }
    }
}

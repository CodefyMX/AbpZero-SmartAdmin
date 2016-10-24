namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class PhoneConfirmationInput
    {
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryPhoneCode { get; set; }
        public string CountryCode { get; set; }
        public long UserId { get; set; }
    }
}

namespace Cinotam.AbpModuleZero.Web.Models.Account
{
    public class UserTwoFactorVerificationInput
    {
        public string ReturnUrl { get; set; }
        public long UserId { get; set; }
        public string Provider { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
    }
}
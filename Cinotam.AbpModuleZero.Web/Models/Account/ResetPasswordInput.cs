namespace Cinotam.AbpModuleZero.Web.Models.Account
{
    public class ResetPasswordInput
    {
        public string Token { get; set; }
        public long UserId { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
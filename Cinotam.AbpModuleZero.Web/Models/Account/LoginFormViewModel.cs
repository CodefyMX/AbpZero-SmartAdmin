namespace Cinotam.AbpModuleZero.Web.Models.Account
{
    public class LoginFormViewModel
    {
        public string ReturnUrl { get; set; }

        public bool IsMultiTenancyEnabled { get; set; }
        public bool IsAValidTenancyNameInUrl { get; set; }
        public string TenancyName { get; set; }
    }
}
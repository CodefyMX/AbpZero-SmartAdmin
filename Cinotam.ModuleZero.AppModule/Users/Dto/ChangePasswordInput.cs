namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class ChangePasswordInput
    {
        public long? UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

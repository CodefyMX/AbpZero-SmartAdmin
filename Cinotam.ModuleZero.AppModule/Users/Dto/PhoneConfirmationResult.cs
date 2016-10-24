namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class PhoneConfirmationResult
    {
        public ConfirmationCodes ConfirmationCodes { get; set; }
        public string Message { get; set; }
    }

    public enum ConfirmationCodes
    {
        Success,
        Error
    }
}

namespace Cinotam.ModuleZero.AppModule.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }
    }

    public class ChatLoginInformation : GetCurrentLoginInformationsOutput
    {
        public int? ConversationId { get; set; }
    }

}
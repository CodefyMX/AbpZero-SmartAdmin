namespace Cinotam.ModuleZero.AppModule.Chat.Dto
{
    public class SendMessageInput
    {
        public long? From { get; set; }
        public long? To { get; set; }

        public string Message { get; set; }

    }
}

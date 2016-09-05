namespace CInotam.MailSender.Contracts
{
    public interface IMail
    {
        string From { get; set; }
        string To { get; set; }
        string Data { get; }
        bool Sent { get; set; }
    }
}

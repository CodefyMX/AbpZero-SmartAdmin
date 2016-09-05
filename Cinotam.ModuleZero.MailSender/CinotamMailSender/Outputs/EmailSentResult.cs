namespace Cinotam.MailSender.CinotamMailSender.Outputs
{
    public class EmailSentResult
    {
        public EmailSentResult()
        {
            SentWithHttp = false;
            SentWithSmtp = false;
        }
        public bool SentWithSmtp { get; set; }
        public bool SentWithHttp { get; set; }
    }
}

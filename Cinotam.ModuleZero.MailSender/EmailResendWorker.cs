using Abp.BackgroundJobs;
using Cinotam.ModuleZero.MailSender.CinotamMailSender;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Inputs;

namespace Cinotam.ModuleZero.MailSender
{
    public class EmailResendWorker : BackgroundJob<EmailSendInput>
    {
        private readonly ICinotamMailSender _cinotamMailSender;

        public EmailResendWorker(ICinotamMailSender cinotamMailSender)
        {
            _cinotamMailSender = cinotamMailSender;
        }

        public override void Execute(EmailSendInput args)
        {

        }
    }
}

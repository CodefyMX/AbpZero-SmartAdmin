using Abp.Domain.Services;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Outputs;
using CInotam.MailSender.Contracts;
using System;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.MailSender.CinotamMailSender
{
    public class CinotamMailSender : DomainService, ICinotamMailSender
    {
        public async Task<IMailServiceResult> SendMail(IMail input)
        {
            foreach (var mailServiceProvider in MailSenderAbpModule.MailServiceProviders)
            {
                var result = await mailServiceProvider.DeliverMail(input);
                if (result.MailSent) return new EmailSentResult()
                {
                    MailSent = true,
                    SentWithSmtp = mailServiceProvider.IsSmtp,
                    SentWithHttp = mailServiceProvider.IsHttp
                };
            }
            throw new InvalidOperationException(nameof(MailSenderAbpModule));

        }
        public async Task<IMailServiceResult> DeliverMail(IMail mail)
        {
            var result = (EmailSentResult)(await SendMail(mail));
            return new EmailSentResult()
            {
                MailSent = result.MailSent,
                SentWithSmtp = result.SentWithSmtp,
                SentWithHttp = result.SentWithHttp
            };
        }
    }
}

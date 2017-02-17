using Abp.Domain.Services;
using Abp.Net.Mail;
using Cinotam.MailSender.SmtpDefault.CinotamMailSender.Outputs;
using CInotam.MailSender.Contracts;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cinotam.MailSender.SmtpDefault.CinotamMailSender
{
    public class CinotamMailSenderDefault : DomainService, ICinotamMailSenderDefault
    {
        private readonly IEmailSender _emailSender;
        public CinotamMailSenderDefault(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<EmailSentResult> SendMail(IMail input)
        {
            var result = new EmailSentResult();

            var resultSmtp = await SendViaSmtp(input.MailMessage);
            //Issue: https://github.com/periface/AbpCinotamZero-SmartAdmin/issues/91
            result.SentWithSmtp = resultSmtp;
            //If was sent via smtp just return
            result.MailSent = resultSmtp;
            if (resultSmtp) return result;

            if (result.SentWithHttp || result.SentWithSmtp)
            {
                result.MailSent = true;
            }
            return result;
        }
        async Task<bool> SendViaSmtp(MailMessage message)
        {
            try
            {
                var useSmtp = bool.Parse((await SettingManager.GetSettingValueAsync("UseSmtp")));

                if (!useSmtp) return false;
                await _emailSender.SendAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IMailServiceResult> DeliverMail(IMail mail)
        {
            var result = await SendMail(mail);
            return new EmailSentResult()
            {
                MailSent = result.MailSent,
                SentWithSmtp = result.SentWithSmtp,
                SentWithHttp = result.SentWithHttp
            };
        }

        public bool IsSmtp => true;
        public bool IsHttp => false;
    }
}

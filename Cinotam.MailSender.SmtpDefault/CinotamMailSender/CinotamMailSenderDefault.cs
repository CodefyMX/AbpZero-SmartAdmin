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
            result.SentWithSmtp = resultSmtp;
            //If was sent via smtp just return
            if (resultSmtp) return result;
            //Implement httpServices here

            //var resultHttp = await SendViaHttp(input);
            //result.SentWithHttp = resultHttp;

            if (result.SentWithHttp || result.SentWithSmtp)
            {
                input.Sent = true;
            }
            return result;
        }

        //async Task<bool> SendViaHttp(IMail input)
        //{

        //    var firstOrDefault = input.MailMessage.To.FirstOrDefault();
        //    if (firstOrDefault == null) return false;
        //    var mailAddress = input.MailMessage.To.FirstOrDefault();
        //    if (mailAddress == null) return false;
        //    var response = await _sendGridService.SendViaHttp(new SendGridMessageInput()
        //    {
        //        Body = input.Body,
        //        EncodeType = input.EncodeType,
        //        Message = input.MailMessage,
        //        Subject = input.MailMessage.Subject,
        //        To = mailAddress.Address,
        //        TemplateId = input.ExtraParams.TemplateId,
        //        Substitutions = input.ExtraParams.Substitutions,
        //    });
        //    return response.Success;
        //}
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

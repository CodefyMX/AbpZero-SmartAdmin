using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Net.Mail;
using Cinotam.AbpModuleZero.SystemMails;
using Cinotam.MailSender.SendGrid.SendGrid;
using Cinotam.MailSender.SendGrid.SendGrid.Inputs;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Inputs;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Outputs;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.MailSender.CinotamMailSender
{
    public class CinotamMailSender : DomainService, ICinotamMailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly ISendGridService _sendGridService;
        private readonly IRepository<SystemMail> _systemMailsRepository;
        public CinotamMailSender(IEmailSender emailSender, ISendGridService sendGridService, IRepository<SystemMail> systemMailsRepository)
        {
            _emailSender = emailSender;
            _sendGridService = sendGridService;
            _systemMailsRepository = systemMailsRepository;
        }

        public async Task<EmailSentResult> SendMail(EmailSendInput input)
        {
            var result = new EmailSentResult();

            var resultSmtp = await SendViaSmtp(input.MailMessage);
            result.SentWithSmtp = resultSmtp;
            //If was sent via smtp just return
            if (resultSmtp) return result;
            //Implement httpServices here

            var resultHttp = await SendViaHttp(input);
            result.SentWithHttp = resultHttp;

            if (result.SentWithHttp || result.SentWithSmtp)
            {
                input.Sent = true;
            }
            await SaveEmail(input);
            return result;
        }

        private async Task SaveEmail(EmailSendInput email)
        {
            await _systemMailsRepository.InsertAndGetIdAsync(new SystemMail()
            {
                User = email.MailMessage.To.ToString(),
                MessageData = email.MailMessage.Body,
                Sent = email.Sent
            });
        }
        async Task<bool> SendViaHttp(EmailSendInput input)
        {

            var firstOrDefault = input.MailMessage.To.FirstOrDefault();
            if (firstOrDefault == null) return false;
            var mailAddress = input.MailMessage.To.FirstOrDefault();
            if (mailAddress == null) return false;
            var response = await _sendGridService.SendViaHttp(new SendGridMessageInput()
            {
                Body = input.Body,
                EncodeType = input.EncodeType,
                Message = input.MailMessage,
                Subject = input.MailMessage.Subject,
                To = mailAddress.Address,
                TemplateId = input.ExtraParams.TemplateId,
                Substitutions = input.ExtraParams.Substitutions,
            });
            return response.Success;
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
    }
}

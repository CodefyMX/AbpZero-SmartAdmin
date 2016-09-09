using Cinotam.MailSender.SendGrid.Credentials;
using Cinotam.MailSender.SendGrid.SendGrid.Outputs;
using CInotam.MailSender.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Mail = SendGrid.Helpers.Mail.Mail;

namespace Cinotam.MailSender.SendGrid.SendGrid
{
    public class SendGridService : ISendGridService
    {
        private readonly SendGridAPIClient _sendGrid;
        public SendGridService(ISendGridCredentialsService sendGridCredentialsService)
        {
            _sendGrid = sendGridCredentialsService.GetInstance("SendGridKey", EnvironmentVariableTarget.Machine);
        }

        public async Task<SendGridMessageResult> SendViaHttp(IMail input)
        {

            var from = new Email(input.MailMessage.From.Address);
            var subject = input.MailMessage.To;
            var to = new Email(input.MailMessage.To.ToString());
            var content = new Content(input.EncodeType, input.Body);
            var mail = new Mail(from, input.MailMessage.Subject, to, content);
            if (!string.IsNullOrEmpty(input.ExtraParams.TemplateId) && input.ExtraParams.EnableTemplates)
            {
                mail.TemplateId = input.ExtraParams.TemplateId;
                if (input.ExtraParams.Substitutions != null)
                {
                    foreach (var substitution in input.ExtraParams.Substitutions)
                    {
                        mail.Personalization[0].AddSubstitution(substitution.Key, substitution.Value);
                    }
                }

            }
            var result = await _sendGrid.client.mail.send.post(requestBody: mail.Get());
            if (result.StatusCode.ToString() == "Accepted")
            {
                return new SendGridMessageResult()
                {
                    ErroMessage = "",
                    Success = true
                };
            }
            return new SendGridMessageResult()
            {
                ErroMessage = result.StatusCode.ToString(),
                Success = false
            };

        }

        public async Task<IMailServiceResult> DeliverMail(IMail mail)
        {
            var result = await SendViaHttp(mail);
            return new SendGridMessageResult()
            {
                MailSent = result.Success
            };
        }

        public bool IsSmtp => false;
        public bool IsHttp => true;
    }
}

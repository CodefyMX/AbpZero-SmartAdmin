using Cinotam.MailSender.SendGrid.Credentials;
using Cinotam.MailSender.SendGrid.SendGrid.Inputs;
using Cinotam.MailSender.SendGrid.SendGrid.Outputs;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Cinotam.MailSender.SendGrid.SendGrid
{
    public class SendGridService : ISendGridService
    {
        private readonly SendGridAPIClient _sendGrid;
        public SendGridService(ISendGridCredentialsService sendGridCredentialsService)
        {
            _sendGrid = sendGridCredentialsService.GetInstance("SendGridKey", EnvironmentVariableTarget.User);
        }

        public async Task<SendGridMessageResult> SendViaHttp(SendGridMessageInput input)
        {
            var from = new Email(input.Message.From.Address);
            var subject = input.Subject;
            var to = new Email(input.To);
            var content = new Content(input.EncodeType, input.Body);
            var mail = new Mail(from, subject, to, content);
            if (!string.IsNullOrEmpty(input.TemplateId))
            {
                mail.TemplateId = input.TemplateId;
                if (input.Substitutions != null)
                {
                    foreach (var substitution in input.Substitutions)
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
    }
}

using SendGrid;
using System;

namespace Cinotam.MailSender.SendGrid.Credentials
{
    /// <summary>
    /// Send grid only requires the api key
    /// </summary>
    public class SendGridCredentialsService : ISendGridCredentialsService
    {
        public SendGridAPIClient GetInstance(string envVar, EnvironmentVariableTarget target)
        {
            var apiKey = Environment.GetEnvironmentVariable(envVar, target);
            var sg = new SendGridAPIClient(apiKey);
            return sg;
        }
    }
}

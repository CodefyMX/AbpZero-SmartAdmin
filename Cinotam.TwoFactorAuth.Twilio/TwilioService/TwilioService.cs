using Cinotam.TwoFactorAuth.Contracts;
using Cinotam.TwoFactorAuth.Twilio.Credentials;
using Cinotam.TwoFactorAuth.Twilio.Credentials.Input;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Twilio;

namespace Cinotam.TwoFactorAuth.Twilio.TwilioService
{
    public class TwilioService : ITwilioService
    {
        private readonly TwilioRestClient _client;
        private const string From = "+12016901854";
        private const string CorrectStatus = "queued";
        public TwilioService(ITwilioSenderCredentials twilioSenderCredentials)
        {
            _client = twilioSenderCredentials.GetClient(new TwilioCredentials()
            {
                ApiKeyVarName = "TApiKey",
                ApiSecretVarName = "TApiSecret",
                EnvTarget = EnvironmentVariableTarget.User
            });
        }
        public Task<SendMessageResult> SendMessage(IdentityMessage message)
        {
            var destinationWithPlus = "+" + message.Destination;
            var result = _client.SendMessage(From, destinationWithPlus, message.Body);
            Trace.TraceInformation(result.Status);

            if (result.Status == CorrectStatus)
            {
                return Task.FromResult(new SendMessageResult()
                {
                    SendStatus = SendStatus.Queued
                });
            }

            if (result.RestException != null)
            {
                return Task.FromResult(new SendMessageResult()
                {
                    SendStatus = SendStatus.Fail,
                    Properties = { ["Error"] = result.RestException.Message, ["ErrorCode"] = result.RestException.Code }
                });
            }

            return Task.FromResult(new SendMessageResult()
            {
                SendStatus = SendStatus.Fail
            });
        }

        public string ServiceName => "Twilio";
        public Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}

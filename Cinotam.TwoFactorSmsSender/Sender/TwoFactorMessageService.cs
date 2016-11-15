using Abp.Domain.Services;
using Cinotam.ModuleZero.MailSender.CinotamMailSender;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Inputs;
using Cinotam.TwoFactorAuth.Contracts;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorSender.Sender
{
    public class TwoFactorMessageService : DomainService, ITwoFactorMessageService
    {
        private readonly ICinotamMailSender _cinotamMailSender;

        public TwoFactorMessageService(ICinotamMailSender cinotamMailSender)
        {
            _cinotamMailSender = cinotamMailSender;
        }

        public async Task<SendMessageResult> SendSmsMessage(IdentityMessage message)
        {
            foreach (var messageSender in TwoFactorSenderModule.MessageSenders.Where(a => a.ServiceName == "Twilio"))
            {
                try
                {
                    var result = await messageSender.SendSmsMessage(message);
                    return result;
                }
                catch (Exception)
                {
                    //
                }
            }
            return new SendMessageResult() { SendStatus = SendStatus.Fail };
        }

        public async Task<SendMessageResult> SendEmailMessage(IdentityMessage message)
        {
            var result = await _cinotamMailSender.DeliverMail(new EmailSendInput()
            {
                Body = message.Body,
                MailMessage = new MailMessage()
                {
                    From = new MailAddress((await SettingManager.GetSettingValueAsync("Abp.Net.Mail.DefaultFromAddress"))),
                    To = { new MailAddress(message.Destination) },
                    Subject = "Cinotam- Two factor code",

                },
                EncodeType = "text/html",

            });
            return new SendMessageResult()
            {
                SendStatus = result.MailSent ? SendStatus.Success : SendStatus.Fail
            };
        }

        public string ServiceName => "";

        public async Task SendAsync(IdentityMessage message)
        {
            await SendSmsMessage(message);
        }
    }
}

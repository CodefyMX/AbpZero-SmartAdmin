using Cinotam.TwoFactorAuth.Contracts;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorSender.Sender
{
    public class TwoFactorMessageService : ITwoFactorMessageService
    {
        public async Task<SendMessageResult> SendMessage(IdentityMessage message)
        {
            foreach (var messageSender in TwoFactorSenderModule.MessageSenders.Where(a => a.ServiceName == "Twilio"))
            {
                try
                {
                    var result = await messageSender.SendMessage(message);
                    return result;
                }
                catch (Exception)
                {
                    //
                }
            }
            return new SendMessageResult() { SendStatus = SendStatus.Fail };
        }

        public string ServiceName { get; }

        public async Task SendAsync(IdentityMessage message)
        {
            await SendMessage(message);
        }
    }
}

using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorSender.Sender
{
    public class TwoFactorMessageService : ITwoFactorMessageService
    {
        public async Task SendMessage(IdentityMessage message)
        {
            foreach (var messageSender in TwoFactorSenderModule.MessageSenders.Where(a => a.ServiceName == "Twilio"))
            {
                try
                {
                    await messageSender.SendMessage(message);
                    return;
                }
                catch (Exception)
                {
                    //
                }
            }
        }

        public string ServiceName { get; }

        public async Task SendAsync(IdentityMessage message)
        {
            await SendMessage(message);
        }
    }
}

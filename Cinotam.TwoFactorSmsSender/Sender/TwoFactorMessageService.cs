using Cinotam.TwoFactorAuth.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.TwoFactorSender.Sender
{
    public class TwoFactorMessageService : ITwoFactorMessageService
    {
        public async Task SendMessage(CinotamAbpIdentityMessage message)
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
    }
}

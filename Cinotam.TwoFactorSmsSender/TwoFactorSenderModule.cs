using Abp.Modules;
using Cinotam.TwoFactorAuth.Contracts;
using Cinotam.TwoFactorAuth.Twilio;
using Cinotam.TwoFactorAuth.Twilio.TwilioService;
using System.Collections.Generic;
using System.Reflection;

namespace Cinotam.TwoFactorSender
{
    [DependsOn(typeof(TwoFactorTwilioModule))]
    public class TwoFactorSenderModule : AbpModule
    {
        public static List<IMessageSender> MessageSenders = new List<IMessageSender>();
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            MessageSenders.Add(IocManager.Resolve<TwilioService>());
        }
    }
}

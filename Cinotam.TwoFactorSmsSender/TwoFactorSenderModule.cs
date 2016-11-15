using Abp.Modules;
using Cinotam.ModuleZero.MailSender;
using Cinotam.TwoFactorAuth.Contracts;
using Cinotam.TwoFactorAuth.Twilio;
using Cinotam.TwoFactorAuth.Twilio.TwilioService;
using System.Collections.Generic;
using System.Reflection;

namespace Cinotam.TwoFactorSender
{
    [DependsOn(typeof(TwoFactorTwilioModule), typeof(CinotamModuleZeroMailSender))]
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

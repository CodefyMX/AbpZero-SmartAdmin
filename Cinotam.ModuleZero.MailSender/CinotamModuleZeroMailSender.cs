using Abp.Modules;
using Cinotam.MailSender.SendGrid;
using Cinotam.MailSender.SendGrid.SendGrid;
using Cinotam.MailSender.SmtpDefault;
using Cinotam.MailSender.SmtpDefault.CinotamMailSender;
using System.Reflection;

namespace Cinotam.ModuleZero.MailSender
{

    [DependsOn(typeof(CinotamMailSenderSendGrid), typeof(CinotamMailSenderSmtp))]
    public class CinotamModuleZeroMailSender : MailSenderAbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            MailServiceProviders.Add(IocManager.Resolve<CinotamMailSenderDefault>());
            MailServiceProviders.Add(IocManager.Resolve<SendGridService>());
        }
    }
}

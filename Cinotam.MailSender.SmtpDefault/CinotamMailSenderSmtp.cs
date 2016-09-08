using Abp.Modules;
using System.Reflection;

namespace Cinotam.MailSender.SmtpDefault
{
    public class CinotamMailSenderSmtp : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

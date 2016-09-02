using Abp.Modules;
using System.Reflection;

namespace Cinotam.MailSender.SendGrid
{

    public class CinotamMailSenderSendGrid : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

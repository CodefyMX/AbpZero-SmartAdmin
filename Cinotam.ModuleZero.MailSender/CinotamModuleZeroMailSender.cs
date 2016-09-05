using System.Reflection;
using Abp.Modules;
using Cinotam.MailSender.SendGrid;

namespace Cinotam.MailSender
{

    [DependsOn(typeof(CinotamMailSenderSendGrid))]
    public class CinotamModuleZeroMailSender : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

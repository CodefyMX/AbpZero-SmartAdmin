using Abp.Modules;
using Cinotam.MailSender.SendGrid;
using System.Reflection;

namespace Cinotam.ModuleZero.MailSender
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

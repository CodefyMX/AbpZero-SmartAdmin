using Abp.Modules;
using System.Reflection;

namespace Cinotam.TwoFactorAuth.Twilio
{
    public class TwoFactorTwilioModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

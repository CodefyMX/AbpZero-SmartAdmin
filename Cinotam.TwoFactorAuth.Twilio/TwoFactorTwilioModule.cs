using Abp.Modules;
using RestApiHelpers;
using System.Reflection;

namespace Cinotam.TwoFactorAuth.Twilio
{
    [DependsOn(typeof(RestApiHelpersModule))]
    public class TwoFactorTwilioModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

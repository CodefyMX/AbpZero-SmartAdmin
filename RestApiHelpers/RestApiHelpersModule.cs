using Abp.Modules;
using System.Reflection;

namespace RestApiHelpers
{
    public class RestApiHelpersModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

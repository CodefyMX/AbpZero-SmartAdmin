using Abp.Modules;
using Cinotam.SimplePost.Core;
using System.Reflection;

namespace Cinotam.SimplePost.Application
{
    [DependsOn(typeof(CinotamSimplePostCoreModule))]
    public class CinotamSimplePostAppModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

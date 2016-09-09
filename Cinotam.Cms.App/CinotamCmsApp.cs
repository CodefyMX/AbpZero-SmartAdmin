using Abp.Modules;
using Cinotam.Cms.Core;
using System.Reflection;

namespace Cinotam.Cms.App
{
    [DependsOn(typeof(CinotamCmsCore))]
    public class CinotamCmsApp : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

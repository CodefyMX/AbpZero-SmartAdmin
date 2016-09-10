using Abp.Modules;
using System.Reflection;

namespace Cinotam.Cms.DatabaseTemplateProvider
{
    public class CinotamCmsDatabaseTemplateProvider : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

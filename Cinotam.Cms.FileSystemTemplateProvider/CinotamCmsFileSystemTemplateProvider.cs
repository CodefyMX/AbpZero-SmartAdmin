using Abp.Modules;
using System.Reflection;

namespace Cinotam.Cms.FileSystemTemplateProvider
{
    public class CinotamCmsFileSystemTemplateProvider : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

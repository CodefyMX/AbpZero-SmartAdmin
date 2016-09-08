using Abp.Modules;
using System.Reflection;

namespace Cinotam.FileManager.Local
{
    public class CinotamFileManagerLocal : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

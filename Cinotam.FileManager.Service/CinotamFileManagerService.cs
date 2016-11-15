using Abp.Modules;
using System.Reflection;

namespace Cinotam.FileManager.Service
{
    [DependsOn(typeof(CinotamFileManager))]
    public class CinotamFileManagerService : AbpModule
    {
        public static bool UseCdn => true;
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

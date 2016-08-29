using Abp.Modules;
using Cinotam.FileManager;
using System.Reflection;

namespace Cinotam.ModuleZero.BackgroundTasks
{
    [DependsOn(typeof(CinotamFileManager))]
    public class CinotamModuleZeroBackground : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

    }
}

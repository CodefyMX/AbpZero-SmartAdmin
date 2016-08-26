using Abp.Modules;
using System.Reflection;

namespace Cinotam.ImageManager
{
    public class CinotamImageModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

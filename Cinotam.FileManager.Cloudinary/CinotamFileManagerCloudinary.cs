using Abp.Modules;
using System.Reflection;

namespace Cinotam.FileManager.Cloudinary
{
    public class CinotamFileManagerCloudinary : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

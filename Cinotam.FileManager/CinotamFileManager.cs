using Abp.Modules;
using Cinotam.FileManager.Cloudinary;
using System.Reflection;

namespace Cinotam.FileManager
{
    [DependsOn(typeof(CinotamFileManagerCloudinary))]
    public class CinotamFileManager : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

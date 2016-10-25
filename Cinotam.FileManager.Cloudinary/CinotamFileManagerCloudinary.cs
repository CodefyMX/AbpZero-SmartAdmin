using Abp.Modules;
using RestApiHelpers;
using System.Reflection;

namespace Cinotam.FileManager.Cloudinary
{
    [DependsOn(typeof(RestApiHelpersModule))]
    public class CinotamFileManagerCloudinary : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

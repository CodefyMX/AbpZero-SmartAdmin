using Abp.Modules;
using Cinotam.FileManager.Cloudinary;
using Cinotam.FileManager.Cloudinary.Cloudinary;
using Cinotam.FileManager.Local;
using Cinotam.FileManager.Local.LocalFileManager;
using System.Reflection;

namespace Cinotam.FileManager
{
    [DependsOn(typeof(CinotamFileManagerCloudinary), typeof(CinotamFileManagerLocal))]
    public class CinotamFileManager : FileManagerModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {

            FileManagerServiceProviders.Add(IocManager.Resolve<LocalFileManager>());
            FileManagerServiceProviders.Add(IocManager.Resolve<CloudinaryApiConsumer>());
        }
    }
}

using Abp.Modules;
using System.Reflection;

namespace Cinotam.ModuleZero.Notifications
{
    public class CinotamModuleZeroNotifications : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

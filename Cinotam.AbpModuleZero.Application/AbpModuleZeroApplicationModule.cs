using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace Cinotam.AbpModuleZero
{
    [DependsOn(typeof(AbpModuleZeroCoreModule), typeof(AbpAutoMapperModule))]
    public class AbpModuleZeroApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

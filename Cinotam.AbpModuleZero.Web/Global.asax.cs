using Abp.Web;
using System;

namespace Cinotam.AbpModuleZero.Web
{
    public class MvcApplication : AbpWebApplication<AbpModuleZeroWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            //Disabled logger
            //AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

            base.Application_Start(sender, e);
        }
    }
}

using Abp.Web;
using System;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web
{
    public class MvcApplication : AbpWebApplication<AbpModuleZeroWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            //Disabled logger
            //AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            //find the default JsonVAlueProviderFactory
            JsonValueProviderFactory jsonValueProviderFactory = null;

            foreach (var factory in ValueProviderFactories.Factories)
            {
                if (factory is JsonValueProviderFactory)
                {
                    jsonValueProviderFactory = factory as JsonValueProviderFactory;
                }
            }

            //remove the default JsonVAlueProviderFactory
            if (jsonValueProviderFactory != null) ValueProviderFactories.Factories.Remove(jsonValueProviderFactory);

            //add the custom one
            ValueProviderFactories.Factories.Add(new CustomJsonValueProviderFactory());
            base.Application_Start(sender, e);
        }
    }
}

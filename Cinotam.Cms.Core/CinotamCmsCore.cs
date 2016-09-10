using Abp.Modules;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseContentProvider;
using Cinotam.Cms.DatabaseTemplateProvider;
using System.Collections.Generic;
using System.Reflection;

namespace Cinotam.Cms.Core
{
    [DependsOn(typeof(CinotamCmsDatabaseProvider), typeof(CinotamCmsDatabaseTemplateProvider))]
    public class CinotamCmsCore : AbpModule
    {
        public static List<IPageContentProvider> PageContentProviders = new List<IPageContentProvider>();
        public static List<ITemplateContentProvider> TemplateContentProviders = new List<ITemplateContentProvider>();
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {

            PageContentProviders.Add(IocManager.Resolve<DatabaseContentProvider.Provider.DatabaseContentProvider>());

            //PageContentProviders.Add(IocManager.Resolve<FileSystemContentProvider.Provider.FileSystemContentProvider>());

            TemplateContentProviders.Add(IocManager.Resolve<DatabaseTemplateProvider.Provider.DatabaseTemplateProvider>());
        }
    }
}

using Abp.Modules;
using Cinotam.AbpModuleZero;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseContentProvider;
using Cinotam.Cms.DatabaseTemplateProvider;
using Cinotam.Cms.FileSystemTemplateProvider;
using System.Collections.Generic;
using System.Reflection;

namespace Cinotam.Cms.Core
{
    [DependsOn(typeof(AbpModuleZeroCoreModule), typeof(CinotamCmsDatabaseProvider), typeof(CinotamCmsDatabaseTemplateProvider), typeof(CinotamCmsFileSystemTemplateProvider))]
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


            TemplateContentProviders.Add(IocManager.Resolve<FileSystemTemplateProvider.Provider.FileSystemTemplateProvider>());

            TemplateContentProviders.Add(IocManager.Resolve<DatabaseTemplateProvider.Provider.DatabaseTemplateProvider>());
        }
    }
}

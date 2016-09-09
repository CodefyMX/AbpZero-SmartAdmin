using Abp.Modules;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseContentProvider;
using System.Collections.Generic;
using System.Reflection;

namespace Cinotam.Cms.Core
{
    [DependsOn(typeof(CinotamCmsDatabaseProvider))]
    public class CinotamCmsCore : AbpModule
    {
        public static List<ITemplateContentProvider> TemplateContentProviders = new List<ITemplateContentProvider>();

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            TemplateContentProviders.Add(IocManager.Resolve<DatabaseContentProvider.Provider.DatabaseContentProvider>());
            TemplateContentProviders.Add(IocManager.Resolve<FileSystemContentProvider.Provider.FileSystemContentProvider>());
        }
    }
}

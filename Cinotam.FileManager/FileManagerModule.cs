using Abp.Modules;
using System;
using System.Collections.Generic;

namespace Cinotam.FileManager
{
    public class FileManagerModule : AbpModule
    {
        public static List<Type> FileManagerServiceProviders = new List<Type>();

        protected FileManagerModule()
        {

        }
    }
}

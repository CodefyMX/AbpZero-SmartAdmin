using Abp.Modules;
using System;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.MailSender
{
    public class MailSenderAbpModule : AbpModule
    {
        public static List<Type> MailServiceProviders = new List<Type>();
        protected MailSenderAbpModule()
        {

        }
    }
}

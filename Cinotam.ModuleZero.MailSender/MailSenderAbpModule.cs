using Abp.Modules;
using CInotam.MailSender.Contracts;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.MailSender
{
    public class MailSenderAbpModule : AbpModule
    {
        public static List<IMailServiceProvider> MailServiceProviders = new List<IMailServiceProvider>();
        protected MailSenderAbpModule()
        {

        }
    }
}

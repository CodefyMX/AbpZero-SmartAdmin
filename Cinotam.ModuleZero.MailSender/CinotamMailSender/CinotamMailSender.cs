using Abp.Dependency;
using Abp.Domain.Services;
using Cinotam.ModuleZero.MailSender.CinotamMailSender.Outputs;
using CInotam.MailSender.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.MailSender.CinotamMailSender
{
    public class CinotamMailSender : DomainService, ICinotamMailSender
    {
        private readonly IocManager _iocManager;

        public CinotamMailSender()
        {
            _iocManager = IocManager.Instance;
        }

        public async Task<IMailServiceResult> SendMail(IMail input)
        {
            var providers = GetProviders();
            foreach (var mailServiceProvider in providers)
            {
                var result = await mailServiceProvider.DeliverMail(input);

                if (!result.MailSent) continue;
                ReleaseAll(providers);
                return new EmailSentResult()
                {
                    MailSent = true,
                    SentWithSmtp = mailServiceProvider.IsSmtp,
                    SentWithHttp = mailServiceProvider.IsHttp
                };
            }
            ReleaseAll(providers);
            throw new InvalidOperationException(nameof(MailSenderAbpModule));

        }
        private void ReleaseAll(List<IMailServiceProvider> providers)
        {
            foreach (var mailServiceProvider in providers)
            {
                Release(mailServiceProvider);
            }
        }

        private void Release(IMailServiceProvider mailServiceProvider)
        {
            _iocManager.Release(mailServiceProvider);
        }

        public async Task<IMailServiceResult> DeliverMail(IMail mail)
        {
            var result = (EmailSentResult)(await SendMail(mail));
            return new EmailSentResult()
            {
                MailSent = result.MailSent,
                SentWithSmtp = result.SentWithSmtp,
                SentWithHttp = result.SentWithHttp
            };
        }

        private List<IMailServiceProvider> GetProviders()
        {
            var providers = new List<IMailServiceProvider>();
            foreach (var mailServiceProvider in MailSenderAbpModule.MailServiceProviders)
            {
                var provider = _iocManager.Resolve(mailServiceProvider);
                providers.Add((IMailServiceProvider)provider);
            }
            return providers;
        }
    }
}

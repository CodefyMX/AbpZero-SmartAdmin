using System.Collections.Generic;
using Abp.Domain.Services;

namespace Cinotam.MailSender.TemplateManager
{
    public interface ITemplateManager : IDomainService
    {
        string GetContent(TemplateType type, string user, string content);
        string GetContent(TemplateType type, IDictionary<int, string> arguments);
    }
}

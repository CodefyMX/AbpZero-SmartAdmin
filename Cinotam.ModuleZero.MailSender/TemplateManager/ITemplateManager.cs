using Abp.Domain.Services;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.MailSender.TemplateManager
{
    public interface ITemplateManager : IDomainService
    {
        string GetContent(TemplateType type, string user, string content);
        string GetContent(TemplateType type, IDictionary<int, string> arguments);
    }
}

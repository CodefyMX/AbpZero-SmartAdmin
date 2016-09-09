using Abp.Domain.Services;

namespace Cinotam.ModuleZero.MailSender.TemplateManager
{
    public interface ITemplateManager : IDomainService
    {
        string GetContent(TemplateType type, bool enablePartials, params string[] arguments);
    }
}

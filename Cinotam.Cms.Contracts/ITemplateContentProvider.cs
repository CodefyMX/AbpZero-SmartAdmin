using System.Threading.Tasks;

namespace Cinotam.Cms.Contracts
{
    public interface ITemplateContentProvider
    {
        Task<string> GetTemplateContent(string templateName);
        Task<string> GetTemplateContent(int templateId);
        Task CreateEditTemplate(ITemplateContent templateContent);
    }
}

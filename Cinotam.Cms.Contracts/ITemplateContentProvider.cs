using System.Threading.Tasks;

namespace Cinotam.Cms.Contracts
{
    public interface ITemplateContentProvider
    {
        Task<string> GetTemplateContent(string templateName);
        Task CreateEditTemplate(ITemplateContent templateContent);
    }
}

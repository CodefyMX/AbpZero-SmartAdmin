using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.Contracts
{
    public interface ITemplateContentProvider
    {
        Task<string> GetTemplateContent(string templateName);
        Task<List<string>> GetAvailableTemplates();
        Task CreateEditTemplate(ITemplateContent templateContent);
        Task<CTemplate> GetTemplateInfo(string templateName);
        Task<List<CTemplate>> GetTemplatesInfo();
        Task AddJsResource(string resourceRoute, string templateName, string description);
        Task AddCssResource(string resourceRoute, string templateName, string description);
        string ServiceName { get; }

    }
}

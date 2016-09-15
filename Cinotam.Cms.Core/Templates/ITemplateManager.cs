using Abp.Domain.Services;
using Cinotam.Cms.Core.Templates.Outputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Templates
{
    public interface ITemplateManager : IDomainService
    {
        Task<TemplateInfo> GetTemplateContentAsync(string templateName);
        Task<List<string>> GetAvailableTemplatesAsync();
        Task AddCssResource(string url, string name, string description);
        Task AddJsResource(string url, string name, string description);
        Task<List<TemplateInfo>> GetTemplateContentsAsync();
        Task<TemplateCreationResult> AddTemplate(TemplateInfo info);
        Task<TemplateCreationResult> EditTemplate(TemplateInfo info);
    }
}

using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Templates
{
    public interface ITemplateManager : IDomainService
    {
        Task<string> GetTemplateContent(string templateName);
        Task<List<string>> GetAvailableTemplates();
    }
}

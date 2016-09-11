using System.Collections.Generic;
using Abp.Domain.Repositories;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
using System.Threading.Tasks;

namespace Cinotam.Cms.DatabaseTemplateProvider.Provider
{
    public class DatabaseTemplateProvider : IDatabaseTemplateProvider
    {
        private readonly IRepository<Template> _templatesRepository;

        public DatabaseTemplateProvider(IRepository<Template> templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public async Task<string> GetTemplateContent(string templateName)
        {
            var content = await _templatesRepository.FirstOrDefaultAsync(a => a.Name == templateName);
            return content == null ? string.Empty : content.Content;
        }

        public Task<List<string>> GetAvailableTemplates()
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetTemplateContent(int templateId)
        {
            var content = await _templatesRepository.FirstOrDefaultAsync(a => a.Id == templateId);
            return content == null ? string.Empty : content.Content;
        }

        public async Task CreateEditTemplate(ITemplateContent templateContent)
        {
            var content = new Template()
            {
                Content = templateContent.Content,
                Name = templateContent.Name,
                FileName = ""
            };
            await _templatesRepository.InsertAndGetIdAsync(content);
        }
    }
}

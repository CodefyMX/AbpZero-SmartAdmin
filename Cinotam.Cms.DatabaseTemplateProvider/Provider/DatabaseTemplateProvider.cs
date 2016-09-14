using Abp.Domain.Repositories;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.DatabaseTemplateProvider.Provider
{
    public class DatabaseTemplateProvider : IDatabaseTemplateProvider
    {
        private readonly IRepository<Template> _templatesRepository;
        private readonly IRepository<Resource> _resourceRepository;

        public DatabaseTemplateProvider(IRepository<Template> templatesRepository, IRepository<Resource> resourceRepository)
        {
            _templatesRepository = templatesRepository;
            _resourceRepository = resourceRepository;
        }

        public async Task<string> GetTemplateContent(string templateName)
        {
            var content = await _templatesRepository.FirstOrDefaultAsync(a => a.Name == templateName);
            return content == null ? string.Empty : content.Content;
        }

        public async Task<List<string>> GetAvailableTemplates()
        {
            var templates = await _templatesRepository.GetAllListAsync();
            return templates.Select(a => a.Name).ToList();
        }
        public async Task CreateEditTemplate(ITemplateContent templateContent)
        {

            await _templatesRepository.InsertAndGetIdAsync(templateContent as Template);
        }

        public async Task<CTemplate> GetTemplateInfo(string templateName)
        {
            var template = await _templatesRepository.FirstOrDefaultAsync(a => a.Name == templateName);
            return new CTemplate()
            {
                Content = template.Content,
                Name = template.Name,
                ResourcesObj = GetResources(template.Id),
            };
        }

        private ICollection<CResource> GetResources(int templateId)
        {
            var templateResources = _resourceRepository.GetAllList(a => a.TemplateObj.Id == templateId);
            return templateResources.Select(a => new CResource()
            {
                ResourceUrl = a.ResourceUrl,
                ResourceType = a.ResourceType
            }).ToList();
        }

        public Task<List<CTemplate>> GetTemplatesInfo()
        {
            throw new System.NotImplementedException();
        }

        public string ServiceName => "Cinotam.Database.Template.Provider";

        public async Task AddJsResource(string resourceRoute, string templateName, string description)
        {
            var template = await GetTemplateInfo(templateName);
            await _resourceRepository.InsertOrUpdateAndGetIdAsync(new Resource()
            {
                ResourceUrl = resourceRoute,
                ResourceType = "js",
                Description = description,
                Template = template
            });
        }

        public async Task AddCssResource(string resourceRoute, string templateName, string description)
        {
            var template = await GetTemplateInfo(templateName);
            await _resourceRepository.InsertOrUpdateAndGetIdAsync(new Resource()
            {
                ResourceUrl = resourceRoute,
                ResourceType = "css",
                Description = description,
                Template = template
            });
        }
    }
}

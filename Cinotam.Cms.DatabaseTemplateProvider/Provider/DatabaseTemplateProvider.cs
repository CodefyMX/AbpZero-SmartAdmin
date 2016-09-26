using Abp.Domain.Repositories;
using Abp.Extensions;
using Cinotam.AbpModuleZero.Extensions;
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

        public bool IsDatabase => true;

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
        public async Task CreateEditTemplate(CTemplate templateContent)
        {

            templateContent.Content = HtmlCleaner.CleanHtml(templateContent.Content);
            var found = _templatesRepository.FirstOrDefault(a => a.Name == templateContent.Name);

            if (found == null)
            {
                await _templatesRepository.InsertOrUpdateAndGetIdAsync(new Template()
                {
                    Content = templateContent.Content,
                    Name = templateContent.Name,
                    IsPartial = templateContent.IsPartial,
                });
            }

            else
            {
                found.Content = templateContent.Content;
                await _templatesRepository.InsertOrUpdateAndGetIdAsync(found);
            }
        }

        public async Task<CTemplate> GetTemplateInfo(string templateName)
        {
            var template = await _templatesRepository.FirstOrDefaultAsync(a => a.Name == templateName);
            return new CTemplate()
            {
                Content = template.Content.IsNullOrEmpty() ? string.Empty : template.Content,
                Name = template.Name,
                ResourcesObj = GetResources(template.Id),
                IsPartial = template.IsPartial,
            };
        }

        private ICollection<CResource> GetResources(int templateId)
        {
            var resourceList = new List<CResource>();
            var template =
                _templatesRepository.GetAllIncluding(a => a.ResourcesObj).FirstOrDefault(a => a.Id == templateId);
            if (template != null)
                resourceList = template.ResourcesObj.Select(a => new CResource()
                {
                    ResourceUrl = a.ResourceUrl,
                    ResourceType = a.ResourceType
                }).ToList();
            return resourceList;
        }

        public async Task<List<CTemplate>> GetTemplatesInfo()
        {
            var templates = await _templatesRepository.GetAllListAsync();
            var templateList = new List<CTemplate>();
            foreach (var template in templates)
            {
                templateList.Add(new CTemplate()
                {
                    Content = template.Content.IsNullOrEmpty() ? string.Empty : template.Content,
                    Name = template.Name,
                    ResourcesObj = GetResources(template.Id),
                    IsPartial = template.IsPartial,
                });
            }
            return templateList;
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
                Template = template,

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

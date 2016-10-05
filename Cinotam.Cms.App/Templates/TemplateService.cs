using Abp.UI;
using Cinotam.AbpModuleZero.Extensions;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Pages.Dto;
using Cinotam.Cms.App.Templates.Dto;
using Cinotam.Cms.Core.Templates;
using Cinotam.Cms.Core.Templates.Outputs;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Templates
{
    public class TemplateService : CinotamCmsAppServiceBase, ITemplateService
    {
        private readonly ITemplateManager _templateManager;
        public TemplateService(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        public async Task<ReturnModel<TemplateInfo>> GetTemplatesTable(RequestModel<object> request)
        {
            var templates = await _templateManager.GetTemplateContentsAsync();
            return new ReturnModel<TemplateInfo>()
            {
                data = templates.ToArray()
            };
        }

        public async Task<TemplateInfo> GetTemplateInfo(string id)
        {
            var template = await _templateManager.GetTemplateContentAsync(id);
            return template;
        }

        public async Task<TemplateInput> GetTemplateModelForEdit(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new TemplateInput()
                {
                    AvaiableTemplatesToCopy = (await _templateManager.GetAvailableTemplatesAsync()).Select(a => new TemplateDto()
                    {
                        Name = a
                    }).ToList()
                };
            }
            var template = await _templateManager.GetTemplateContentAsync(name);
            return new TemplateInput()
            {
                TemplateName = template.Name,
                Content = template.Content,
                IsPartial = template.IsPartial,
                AvaiableTemplatesToCopy = (await _templateManager.GetAvailableTemplatesAsync()).Select(a => new TemplateDto()
                {

                    Name = a,
                }).ToList()
            };
        }
        public async Task AddTemplate(TemplateInput input)
        {
            if (string.IsNullOrEmpty(input.Content)) input.Content = string.Empty;

            if (!string.IsNullOrEmpty(input.CopyFrom))
            {
                input.Content = (await _templateManager.GetTemplateContentAsync(input.CopyFrom)).Content;
            }

            var template = await _templateManager.GetTemplateContentAsync(input.TemplateName);
            if (template != null)
            {
                template.Content = input.Content;
                var resultEdit = await _templateManager.EditTemplate(template);
                if (resultEdit.HasError)
                {
                    throw new UserFriendlyException(L(resultEdit.ErrorMessage));
                }
                return;
            }

            var result = await _templateManager.AddTemplate(new TemplateInfo()
            {
                Content = input.Content,
                IsPartial = input.IsPartial,
                Name = input.TemplateName.Sluggify(),
            });
            if (result.HasError)
            {
                throw new UserFriendlyException(L(result.ErrorMessage));
            }
        }
    }
}

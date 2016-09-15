using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Templates.Dto;
using Cinotam.Cms.Core.Templates.Outputs;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Templates
{
    public interface ITemplateService : IApplicationService
    {
        Task<ReturnModel<TemplateInfo>> GetTemplatesTable(RequestModel<object> request);
        Task<TemplateInfo> GetTemplateInfo(string id);
        Task AddTemplate(TemplateInput input);
        Task<TemplateInput> GetTemplateModelForEdit(string name = "");
    }
}

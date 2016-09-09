using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Pages.Dto;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Pages
{
    public interface IPagesService : IApplicationService
    {
        Task CreateEditPage(string name, int parent, int templateId);
        Task<PageDto> GetPage(int id, string lang);
        Task<PageDto> GetPreviewPage(int id, string name);
        Task<ReturnModel<PageDto>> GetPageList(RequestModel<object> input);
    }
}

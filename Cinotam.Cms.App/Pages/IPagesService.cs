using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Pages.Dto;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Pages
{
    public interface IPagesService : IApplicationService
    {
        Task CreateEditPage(PageInput input);
        Task<PageInput> GetPage(int? id);
        Task<PageInput> GetPageForEdit(int? id);
        Task<PageDto> GetPreviewPage(int id, string name);
        ReturnModel<PageDto> GetPageList(RequestModel<object> input);
        Task<PageConfigurationObject> GetPageConfigurationObject(int id);
        Task<PageTitleInput> GetPageTitleForEdit(int id, string lang);
        Task CreateEditPageTitle(PageTitleInput input);
        Task<PageViewOutput> GetPageViewById(int id, string lang);
        Task<PageViewOutput> GetPageViewBySlug(string slug);
        Task SavePageContent(PageContentInput input);
        void TogglePageStatus(int pageId);
        void SetPageAsMain(int pageId);
        Task<string> GetMainPageSlug();
        Menu GetPagesMenu();

    }
}

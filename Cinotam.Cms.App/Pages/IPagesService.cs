using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Pages.Dto;
using System.Collections.Generic;
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
        Task TogglePageStatus(int pageId);
        Task TogglePageInMenuStatus(int pageId);
        void SetPageAsMain(int pageId);
        Task<string> GetMainPageSlug();
        Menu GetPagesMenu();

        Task<List<PageDto>> GetPageListDto();
        Task<CategorySetResult> SetCategory(CategoryAssignationInput input);
        Task<CategoryOutput> GetCategories();
        Task<List<Chunk>> GetChunks(ChunkRequest input);
        Task<PageViewOutput> GetTemplateHtml(int id, string lang, string templateName);
        Task SetParentPage(ParentPageSetInput input);
        Task<ImageAddedResult> AddImageToPage(AddImageInput input);
        Task<ImageAddedResult> ProcessImage(ProcessImageInput input);
        Task<string> SaveImageFromBase64(Base64Input base64);
        Task SetTemplate(SetTemplateInput input);
        Task DeletePage(int pageId);
    }
}

using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Languages.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Languages
{
    public interface ILanguageAppService : IApplicationService
    {
        Task AddLanguage(LanguageInput input);
        ReturnModel<LanguageDto> GetLanguagesForTable(RequestModel input);
        LanguageTextsForEdit GetLocalizationTexts(LanguageTextsForEditRequest request);
    }
}

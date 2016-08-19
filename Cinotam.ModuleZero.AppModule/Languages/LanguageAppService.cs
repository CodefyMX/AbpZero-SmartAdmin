using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Languages.Dto;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Languages
{
    public class LanguageAppService : CinotamModuleZeroAppServiceBase, ILanguageAppService
    {
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IApplicationLanguageTextManager _applicationLanguageTextManager;
        private readonly IRepository<ApplicationLanguageText, long> _languageTextsRepository;
        private readonly IRepository<ApplicationLanguage> _languagesRepository;
        public LanguageAppService(IApplicationLanguageManager applicationLanguageManager, IApplicationLanguageTextManager applicationLanguageTextManager, IRepository<ApplicationLanguageText, long> languageTextsRepository, IRepository<ApplicationLanguage> languagesRepository)
        {
            _applicationLanguageManager = applicationLanguageManager;
            _applicationLanguageTextManager = applicationLanguageTextManager;
            _languageTextsRepository = languageTextsRepository;
            _languagesRepository = languagesRepository;
        }
        /// <summary>
        /// Adds a new available language to the app
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddLanguage(LanguageInput input)
        {
            await _applicationLanguageManager.AddAsync(new ApplicationLanguage(AbpSession.TenantId, input.LangCode, input.Icon));
        }

        public ReturnModel<LanguageDto> GetLanguagesForTable(RequestModel input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                int totalCount;

                var allLanguages = _languagesRepository.GetAll();
                var filterByLength = GenerateTableModel(input, allLanguages, "Name", out totalCount).ToList();

                return new ReturnModel<LanguageDto>()
                {
                    draw = input.draw,
                    iTotalDisplayRecords = totalCount,
                    recordsTotal = totalCount,
                    iTotalRecords = allLanguages.Count(),
                    recordsFiltered = filterByLength.Count,
                    length = input.length,
                    data = filterByLength.Select(a => new LanguageDto()
                    {
                        Icon = a.Icon,
                        Name = a.DisplayName,
                        Id = a.Id,
                        CreationTime = a.CreationTime
                    }).ToArray()
                };
            }

        }

        public async Task AddEditLocalizationText(LocalizationTextInput input)
        {
            await _applicationLanguageTextManager.UpdateStringAsync(
                AbpSession.TenantId,
                AbpModuleZeroConsts.LocalizationSourceName,
                CultureInfo.GetCultureInfo(input.Culture),
                input.Key, input.Value);
        }

        public LanguageTextsForEdit GetLocalizationTexts(LanguageTextsForEditRequest input)
        {
            var languageTextsSource = _languageTextsRepository.GetAll().Where(a => a.Source == input.Source && a.LanguageName == input.SourceLang);
            var languageTextsTarget = _languageTextsRepository.GetAll().Where(a => a.Source == input.Target && a.LanguageName == input.TargetLang);

            //Ahora a comparar

            var targetLanguageTexts = new List<LanguageText>();

            var sourceLanguageTexts = new List<LanguageText>();
            foreach (var applicationLanguageTextSrc in languageTextsSource)
            {

                //Adds source 
                sourceLanguageTexts.Add(new LanguageText()
                {
                    Key = applicationLanguageTextSrc.Key,
                    Value = applicationLanguageTextSrc.Value
                });



                //Buscar coincidencia en target
                var element = languageTextsTarget.FirstOrDefault(a => a.Key == applicationLanguageTextSrc.Key);
                if (element == null)
                {
                    //Adds an empty field
                    targetLanguageTexts.Add(new LanguageText()
                    {
                        Key = applicationLanguageTextSrc.Key,
                        Value = ""
                    });
                }
                else
                {
                    //Found adds the current value
                    targetLanguageTexts.Add(new LanguageText()
                    {
                        Key = element.Key,
                        Value = element.Value
                    });
                }
            }
            return new LanguageTextsForEdit()
            {
                Source = input.Source,
                Target = input.Target,
                SourceLanguageTexts = sourceLanguageTexts,
                TargetLanguageTexts = targetLanguageTexts
            };
        }
    }
}

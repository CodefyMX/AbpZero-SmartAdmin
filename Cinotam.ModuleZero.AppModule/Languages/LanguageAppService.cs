﻿using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Runtime.Caching;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Localization;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Languages.Dto;
using Cinotam.ModuleZero.Notifications.LanguagesAppNotifications.Sender;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Languages
{
    [AbpAuthorize(PermissionNames.PagesSysAdminLanguages)]
    public class LanguageAppService : CinotamModuleZeroAppServiceBase, ILanguageAppService
    {
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IApplicationLanguageTextManager _applicationLanguageTextManager;
        private readonly IRepository<ApplicationLanguageText, long> _languageTextsRepository;
        private readonly IRepository<ApplicationLanguage> _languagesRepository;
        private readonly ILanguageTextsProvider _languageTextsProvider;
        private readonly ICacheManager _cacheManager;
        public const string DefaultLanguage = "en";
        private readonly ILanguagesAppNotificationSender _languagesAppNotificationSender;
        public LanguageAppService(IApplicationLanguageManager applicationLanguageManager,
            IRepository<ApplicationLanguageText, long> languageTextsRepository,
            IRepository<ApplicationLanguage> languagesRepository,
            ILanguageTextsProvider languageTextsProvider,
            IApplicationLanguageTextManager applicationLanguageTextManager, ILanguagesAppNotificationSender languagesAppNotificationSender, ICacheManager cacheManager)
        {
            _applicationLanguageManager = applicationLanguageManager;
            _languageTextsRepository = languageTextsRepository;
            _languagesRepository = languagesRepository;
            _languageTextsProvider = languageTextsProvider;
            _applicationLanguageTextManager = applicationLanguageTextManager;
            _languagesAppNotificationSender = languagesAppNotificationSender;
            _cacheManager = cacheManager;
        }
        /// <summary>
        /// Adds a new available language to the app
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddLanguage(LanguageInput input)
        {
            var newLanguage = new ApplicationLanguage(AbpSession.TenantId, input.LangCode, input.DisplayName, input.Icon);
            await _applicationLanguageManager.AddAsync(newLanguage);

            await _languagesAppNotificationSender.SendLanguageCreatedNotification(newLanguage, (await GetCurrentUserAsync()));

        }


        public async Task <ReturnModel<LanguageDto>> GetLanguagesForTable(RequestModel<object> input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                int totalCount;
                var search = new List<Expression<Func<ApplicationLanguage, string>>> { a => a.DisplayName, a => a.Name, a => a.Icon };

                var allLanguages = _languagesRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId || a.TenantId == null);
                var filterByLength = GenerateTableModel(input, allLanguages, search, "Name", out totalCount).ToList();
                var isTenancyUser = (await GetCurrentUserAsync()).TenantId.HasValue;
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
                        DisplayName = a.DisplayName,
                        Name = a.Name,
                        Id = a.Id,
                        CreationTime = a.CreationTime,
                        IsStatic = (a.TenantId == null && isTenancyUser)
                    }).ToArray()
                };
            }

        }

        public async Task DeleteLanguage(string code)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                //Much texts, many memory
                DeleteAllTextsFromLanguage(code);
                var language = _languagesRepository.FirstOrDefault(a => a.Name == code);
                await _applicationLanguageManager.RemoveAsync(AbpSession.TenantId, code);
                await _languagesAppNotificationSender.SendLanguageDeletedNotification(language, (await GetCurrentUserAsync()));
            }
        }

        private void DeleteAllTextsFromLanguage(string code)
        {
            var texts = _languageTextsRepository.GetAllList(a => a.LanguageName == code);
            foreach (var applicationLanguageText in texts)
            {
                _languageTextsRepository.Delete(applicationLanguageText);
            }
        }

        public async Task AddEditLocalizationText(LocalizationTextInput input)
        {
            await _applicationLanguageTextManager.UpdateStringAsync(
                AbpSession.TenantId,
                input.Source,
                CultureInfo.GetCultureInfo(input.LanguageName),
                input.Key, input.Value);
        }

        public async Task UpdateLanguageFromXml(string languageName, string source, bool updateExistingValues = false)
        {
            //Clear cache 
            await ClearCache("AbpLocalizationScripts");
            await ClearCache("AbpZeroLanguages");

            //var languageTexts = _languageTextsProvider.GetTexts(languageName, source);


            //foreach (var languageText in languageTexts)
            //{
            //    //Find keys and add the value
            //    var languageTextFromDb = await _languageTextsRepository.FirstOrDefaultAsync(a => a.LanguageName == languageName && a.Source == source && a.Key == languageText.Key);

            //    if (languageTextFromDb == null)
            //    {
            //        _languageTextsRepository.Insert(new ApplicationLanguageText()
            //        {
            //            LanguageName = languageName,
            //            Source = source,
            //            Key = languageText.Key,
            //            Value = languageText.Value
            //        });
            //    }
            //    else
            //    {
            //        if (languageTextFromDb.Value == "")
            //        {
            //            languageTextFromDb.Value = languageText.Value;
            //        }
            //        else
            //        {
            //            if (updateExistingValues) languageTextFromDb.Value = languageText.Value;
            //        }
            //    }
            //}

        }
        private Task ClearCache(string cacheName)
        {
            var cache = _cacheManager.GetCache(cacheName);

            return cache.ClearAsync();

        }


        /// <summary>
        /// Experimental (Todo:Find the way to make just one ajax call to this function per operation)
        /// Note: if there is a lot of language text elements in the table this may cause some perf. issues
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ReturnModel<LanguageTextTableElement> GetLocalizationTexts(RequestModel<LanguageTextsForEditRequest> input)
        {
            //1.-Load all source texts
            //1.1 If the current tenant has no texts in the dabatabase for the source
            //    we must generate these texts with a default language wich is always available 

            var sourceWasUpdated = false;
            //Source requested

            //Problem repository will return 0 records on new database
            //We need to check if there are texts on the cache

            var languageTextsSource = _languageTextsRepository.GetAll()
                .Where(a => a.Source == input.TypeOfRequest.Source
                && a.LanguageName == input.TypeOfRequest.SourceLang).ToList();

            var languageTextsSourceFromXml = _languageTextsProvider.GetTexts(input.TypeOfRequest.SourceLang, input.TypeOfRequest.Source);



            //var isXmlAvailable = _languageTextsProvider.IsXMLAvailableForTheLangCode(input.TypeOfRequest.SourceLang,
            //    input.TypeOfRequest.Source);

            if (!languageTextsSourceFromXml.Any() && !languageTextsSource.Any()
                /* we need something like '&& IsInCache() '*/)
            {

                //This will restore all the keys from the xml file en should be always available
                _languageTextsProvider.SetLocalizationKeys(input.TypeOfRequest.SourceLang, input.TypeOfRequest.Source,
                    AbpSession.TenantId);
                sourceWasUpdated = true;
            }
            else
            {
                languageTextsSource = languageTextsSourceFromXml.Select(a => new ApplicationLanguageText()
                {
                    Key = a.Key,
                    Value = a.Value,
                    Source = input.TypeOfRequest.Source
                }).ToList();
            }
            //2.-Load all target texts
            //2.1.-If the current tenant has no texts in the database for the target
            //     we must generate them with empty spaces and the key from the source 
            //Texts to edit
            var languageTextsTarget = _languageTextsRepository.GetAll()
                .Where(a => a.Source == input.TypeOfRequest.Source
                && a.LanguageName == input.TypeOfRequest.TargetLang).ToList();

            if (input.TypeOfRequest.TargetLang == input.TypeOfRequest.SourceLang)
            {
                languageTextsTarget = languageTextsSource;
            }

            if (!languageTextsTarget.Any())
            {
                //Only sets keys with empty values
                _languageTextsProvider.SetLocalizationKeys(input.TypeOfRequest.TargetLang, input.TypeOfRequest.Source, AbpSession.TenantId);
                var languageTextsSourceS = _languageTextsRepository.GetAll()
                    .Where(a => a.Source == input.TypeOfRequest.Source
                                && a.LanguageName == input.TypeOfRequest.SourceLang).ToList();


                //Once reloaded we build the model
                var languageTextsTargetS = _languageTextsRepository.GetAll()
                .Where(a => a.Source == input.TypeOfRequest.Source
                && a.LanguageName == input.TypeOfRequest.TargetLang).ToList();
                ////3.-Pupulate table with both key targetText - value and sourceText - value
                return GetTableData(languageTextsTargetS, languageTextsSourceS);
            }
            if (sourceWasUpdated)
            {
                var sourceUpdated = _languageTextsRepository.GetAll()
                    .Where(a => a.Source == input.TypeOfRequest.Source
                                && a.LanguageName == input.TypeOfRequest.SourceLang).ToList();
                return GetTableData(languageTextsTarget, sourceUpdated);
            }
            ////3.-Pupulate table with both key targetText - value and sourceText - value
            return GetTableData(languageTextsTarget, languageTextsSource);
        }


        private ReturnModel<LanguageTextTableElement> GetTableData(List<ApplicationLanguageText> languageTextsTarget, List<ApplicationLanguageText> languageTextsSource)
        {
            var listOfElements = new List<LanguageTextTableElement>();
            foreach (var applicationLanguageText in languageTextsSource)
            {
                listOfElements.Add(new LanguageTextTableElement()
                {
                    Key = applicationLanguageText.Key,
                    Source = applicationLanguageText.Source,
                    SourceValue = applicationLanguageText.Value,
                    TargetValue = GetTargetValueFromList(languageTextsTarget, applicationLanguageText.Key, applicationLanguageText.Value)
                });
            }
            return new ReturnModel<LanguageTextTableElement>()
            {
                data = listOfElements.ToArray()
            };

        }

        private string GetTargetValueFromList(List<ApplicationLanguageText> languageTextsTarget, string key, string sourceValue)
        {
            if (languageTextsTarget.All(a => a.Key != key))
            {
                return sourceValue;
            }
            var first = languageTextsTarget.FirstOrDefault(a => a.Key == key);
            return string.IsNullOrEmpty(first?.Value) ? sourceValue : first.Value;
        }

        public LanguageTextsForEditView GetLanguageTextsForEditView(string selectedTargetLanguage,
            string selectedSourceLanguage)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var result = _languageTextsProvider.GetLocalizationSources();
                var languages = _languagesRepository.GetAll().ToList();
                return new LanguageTextsForEditView()
                {
                    SourceLanguages = languages.Select(a => new LanguageSelected(a.DisplayName, a.Name, a.Icon)).ToList(),
                    TargetLanguages = languages.Select(a => new LanguageSelected(a.DisplayName, a.Name, a.Icon)).ToList(),
                    SelectedSourceLanguage = selectedSourceLanguage,
                    SelectedTargetLanguage = selectedTargetLanguage,
                    Source = result
                };
            }
        }
        #region UnusedCode

        /*For GetLocalizationTexts
         
            Main Idea:

                source = String
                sources = List<LanguageText.cs>
                targets = List<LanguageText.cs>
            
            Params: 
            
                object <LanguageTextsForEditRequest.cs>

            Returns:

                LanguageTextsForEdit.cs

            Why Unused:

                -This will require one additional list
                -Its not suitable for table elements
                -Too hard to implement with datatables.js

            //////////////////////////////

            var languageTextsSource = _languageTextsRepository.GetAll().Where(a => a.Source == input.TypeOfRequest.Source && a.LanguageName == input.TypeOfRequest.SourceLang);
            
            var languageTextsTarget = _languageTextsRepository.GetAll().Where(a => a.Source == input.TypeOfRequest.Source && a.LanguageName == input.TypeOfRequest.TargetLang);

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
                Source = input.TypeOfRequest.Source,
                SourceLanguageTexts = sourceLanguageTexts,
                TargetLanguageTexts = targetLanguageTexts
            };

            //////////////////////////////

         */


        #endregion
    }
}

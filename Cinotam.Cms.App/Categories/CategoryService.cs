using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Localization;
using Abp.Threading;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Extensions;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Categories.Dto;
using Cinotam.Cms.App.Events;
using Cinotam.Cms.App.Pages.Dto;
using Cinotam.Cms.Core.Category;
using Cinotam.Cms.DatabaseEntities.Category.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Categories
{
    public class CategoryService : CinotamCmsAppServiceBase, ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<CategoryContent> _categoryContentRepository;
        private readonly ICategoryManager _categoryManager;
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        public IEventBus EventBus { get; set; }
        public CategoryService(IRepository<Category> categoryRepository, IRepository<CategoryContent> categoryContentRepository, IApplicationLanguageManager applicationLanguageManager, ICategoryManager categoryManager)
        {
            _categoryRepository = categoryRepository;
            _categoryContentRepository = categoryContentRepository;
            _applicationLanguageManager = applicationLanguageManager;
            _categoryManager = categoryManager;
            EventBus = NullEventBus.Instance;

        }

        public async Task AddEditCategory(CategoryInput input)
        {
            var id = await _categoryManager.AddEditCategory(input.Name.Sluggify(), input.Name);
            var categoryCreated = _categoryRepository.FirstOrDefault(id);
            foreach (var inputLanguageInput in input.LanguageInputs)
            {

                if (string.IsNullOrEmpty(inputLanguageInput.Text)) continue;
                var foundWithSameLanguage =
                    _categoryContentRepository.FirstOrDefault(
                        a => a.CategoryId == id && a.Lang == inputLanguageInput.Lang);
                if (foundWithSameLanguage == null)
                {
                    await
                        _categoryManager.AddEditCategoryContent(
                            CategoryContent.CreateCategoryContent(inputLanguageInput.Lang, inputLanguageInput.Text,
                                categoryCreated));
                }
                else
                {
                    foundWithSameLanguage.DisplayText = inputLanguageInput.Text;
                    await _categoryManager.AddEditCategoryContent(foundWithSameLanguage);
                }
            }
        }

        public ReturnModel<CategoryDto> GetCategories(RequestModel<object> requestModel)
        {
            int count;
            var categories = _categoryRepository.GetAll();
            List<Expression<Func<Category, string>>> searchs = new EditableList<Expression<Func<Category, string>>>();
            searchs.Add(a => a.Name);
            searchs.Add(a => a.DisplayName);

            var filteredElements = GenerateTableModel(requestModel, categories, searchs, "Name", out count);
            return new ReturnModel<CategoryDto>()
            {
                data = filteredElements.Select(a => new CategoryDto()
                {
                    Id = a.Id,
                    Languages = AsyncHelper.RunSync(() => GetAvailableLangs(a.Id)),
                    Name = a.Name,
                    DisplayName = a.DisplayName
                }).ToArray(),
                draw = requestModel.draw,
                length = requestModel.length,
                recordsTotal = count,
                iTotalDisplayRecords = count,
                iTotalRecords = categories.Count(),
                recordsFiltered = categories.Count()
            };
        }

        public async Task<CategoryForEditModel> GetCategoryForEdit(int? id)
        {
            if (!id.HasValue) return await CreateEmptyCategoryForEditModel();
            var category = await _categoryRepository.GetAsync(id.Value);

            return new CategoryForEditModel()
            {
                DisplayName = category.DisplayName,
                Name = category.Name,
                CategoryLangContents = await GetLanguageContents(id.Value)
            };

        }

        public async Task RemoveCategory(int categoryId)
        {

            await _categoryRepository.DeleteAsync(categoryId);

            EventBus.Trigger(new CategoryDeletedEventData() { CategoryId = categoryId });
        }
        private async Task<List<CategoryLangContent>> GetLanguageContents(int idValue)
        {
            var allLanguages = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            var categoryLangContent = new List<CategoryLangContent>();
            foreach (var applicationLanguage in allLanguages)
            {
                var contentWithLanguage =
                    _categoryContentRepository.FirstOrDefault(a => a.Lang.Equals(applicationLanguage.Name) && a.CategoryId == idValue);
                if (contentWithLanguage == null)
                {
                    categoryLangContent.Add(new CategoryLangContent()
                    {
                        DisplayText = string.Empty,
                        Icon = applicationLanguage.Icon,
                        Lang = applicationLanguage.Name
                    });
                }
                else
                {
                    categoryLangContent.Add(new CategoryLangContent()
                    {
                        DisplayText = contentWithLanguage.DisplayText,
                        Icon = applicationLanguage.Icon,
                        Lang = applicationLanguage.Name
                    });
                }
            }
            return categoryLangContent;
        }

        private async Task<CategoryForEditModel> CreateEmptyCategoryForEditModel()
        {
            var allLanguages = await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId);
            return new CategoryForEditModel()
            {
                CategoryLangContents = allLanguages.Select(a => new CategoryLangContent()
                {
                    DisplayText = string.Empty,
                    Lang = a.Name,
                    Icon = a.Icon
                }).ToList()
            };

        }

        private async Task<List<Lang>> GetAvailableLangs(int argId)
        {
            var list = new List<Lang>();
            var allContents = _categoryContentRepository.GetAllList(a => a.CategoryId == argId);
            foreach (var item in allContents)
            {
                var lang = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId)).FirstOrDefault(a => a.Name.Equals(item.Lang));
                if (lang != null)
                    list.Add(new Lang()
                    {
                        LangCode = lang.Name,
                        LangIcon = lang.Icon
                    });
            }
            return list;
        }
    }
}

using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Threading;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Categories.Dto;
using Cinotam.Cms.App.Pages.Dto;
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
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        public CategoryService(IRepository<Category> categoryRepository, IRepository<CategoryContent> categoryContentRepository, IApplicationLanguageManager applicationLanguageManager)
        {
            _categoryRepository = categoryRepository;
            _categoryContentRepository = categoryContentRepository;
            _applicationLanguageManager = applicationLanguageManager;
        }

        public Task AddEditCategory(CategoryInput input)
        {
            throw new NotImplementedException();
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

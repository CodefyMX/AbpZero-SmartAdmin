using Castle.Components.DictionaryAdapter;
using Cinotam.Cms.Core.Templates.Outputs;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageConfigurationObject
    {
        public bool IsActive { get; set; }
        public string PageName { get; set; }
        public List<PageContentDto> ContentsByLanguage { get; set; } = new EditableList<PageContentDto>();
        public List<PageDto> AvailablePages { get; set; } = new EditableList<PageDto>();
        public List<TemplateInfo> AvailableTemplates { get; set; } = new EditableList<TemplateInfo>();
        public int Id { get; set; }
        public bool IsMainPage { get; set; }
        public bool IncludeInMenu { get; set; }
        public string CategoryName { get; set; }
        public string TemplateName { get; set; }
        public List<CategoryDto> AvailableCategoryDtos { get; set; } = new EditableList<CategoryDto>();
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public bool ShowBreadCrum { get; set; }
        public bool BreadCrumInContainer { get; set; }
    }
}

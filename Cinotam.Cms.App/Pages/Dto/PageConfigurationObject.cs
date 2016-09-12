using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageConfigurationObject
    {
        public bool IsActive { get; set; }
        public string PageName { get; set; }
        public List<PageContentDto> ContentsByLanguage { get; set; } = new EditableList<PageContentDto>();
        public int Id { get; set; }
        public bool IsMainPage { get; set; }
    }
}

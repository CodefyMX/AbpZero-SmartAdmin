using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageInput
    {
        public string Title { get; set; }
        public int? TemplateId { get; set; }
        public int? ParentId { get; set; }
        public bool Active { get; set; }
        public List<TemplateDto> Templates { get; set; } = new EditableList<TemplateDto>();
        public List<PageDto> Pages { get; set; } = new EditableList<PageDto>();
        public string TemplateName { get; set; }
    }
}

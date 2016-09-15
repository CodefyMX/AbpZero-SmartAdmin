using Castle.Components.DictionaryAdapter;
using Cinotam.Cms.App.Pages.Dto;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Templates.Dto
{
    public class TemplateInput
    {
        public string TemplateName { get; set; }
        public string Content { get; set; }
        public string CopyFrom { get; set; }
        public bool IsPartial { get; set; }
        public List<TemplateDto> AvaiableTemplatesToCopy { get; set; } = new EditableList<TemplateDto>();
    }
}

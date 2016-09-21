using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageDto : EntityDto
    {
        public string Title { get; set; }
        public int TemplateId { get; set; }
        public List<Lang> Langs { get; set; } = new EditableList<Lang>();
        public string CategoryName { get; set; }
        public string TemplateName { get; set; }
    }
}

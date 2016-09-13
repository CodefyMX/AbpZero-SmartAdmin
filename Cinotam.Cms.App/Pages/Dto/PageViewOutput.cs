using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageViewOutput : EntityDto
    {
        public string Lang { get; set; }
        public string HtmlContent { get; set; }
        public string TemplateName { get; set; }
        public string Title { get; set; }
        public List<BreadCrum> BreadCrums { get; set; } = new EditableList<BreadCrum>();
    }
}

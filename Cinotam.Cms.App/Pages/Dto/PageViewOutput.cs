using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using Cinotam.Cms.FileSystemTemplateProvider.Provider.JsonEntities;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageViewOutput : EntityDto
    {
        public string Lang { get; set; }
        public string HtmlContent { get; set; }
        public string TemplateName { get; set; }
        public string Title { get; set; }
        public bool IsPartial { get; set; }
        public List<BreadCrum> BreadCrums { get; set; } = new EditableList<BreadCrum>();
        public int ContentId { get; set; }
        public List<ResourceDto> CssResource { get; set; } = new EditableList<ResourceDto>();
        public List<ResourceDto> JsResource { get; set; } = new EditableList<ResourceDto>();
        public bool ShowBreadCrum { get; set; }
        public bool BreadCrumInContainer { get; set; }
    }
    public class ResourceDto : Resource { }
}

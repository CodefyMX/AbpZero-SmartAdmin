using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageViewOutput : EntityDto
    {
        public string Lang { get; set; }
        public string HtmlContent { get; set; }
        public string TemplateName { get; set; }
        public string Title { get; set; }
    }
}

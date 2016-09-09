using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageDto : EntityDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string TemplateId { get; set; }
        public string Lang { get; set; }
    }
}

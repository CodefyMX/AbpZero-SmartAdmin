using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageContentDto : EntityDto
    {
        public string Lang { get; set; }
        public string HtmlContent { get; set; }
        /// <summary>
        /// Helper for filesystem templates
        /// </summary>
        public string TemplateUniqueName { get; set; }
        public string LanguageIcon { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Preview { get; set; }
    }
}

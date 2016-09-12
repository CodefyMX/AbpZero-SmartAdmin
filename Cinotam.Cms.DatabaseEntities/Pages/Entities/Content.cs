using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    public class Content : FullAuditedEntity, IPageContent
    {
        public string Lang { get; set; }
        public int PageId { get; set; }
        public string HtmlContent { get; set; }
        public string Title { get; set; }
        public virtual Page Page { get; set; }
        /// <summary>
        /// Helper for filesystem templates
        /// </summary>
        public string TemplateUniqueName { get; set; }
        public string Url { get; set; }

    }
}

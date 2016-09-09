using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    public class Content : FullAuditedEntity, IPageContent
    {
        public string Lang { get; set; }
        public int PageId { get; set; }
        public string HtmlContent { get; set; }
        public virtual Page Page { get; set; }
        public virtual Template Template { get; set; }

    }
}

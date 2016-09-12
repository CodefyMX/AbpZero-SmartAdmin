using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    public class Page : FullAuditedEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual int? ParentPage { get; set; }
        public virtual ICollection<Page> ChildPages { get; set; }
        public bool Active { get; set; }
        public string TemplateName { get; set; }
        public bool IsMainPage { get; set; }
    }
}

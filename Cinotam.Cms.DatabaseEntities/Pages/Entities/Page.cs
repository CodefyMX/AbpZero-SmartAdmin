using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
using System.Collections.Generic;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    public class Page : FullAuditedEntity
    {
        public string Name { get; set; }
        public Template Template { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual int? ParentPage { get; set; }
        public virtual ICollection<Page> ChildPages { get; set; }
    }
}

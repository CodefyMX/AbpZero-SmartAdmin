using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Collections.Generic;

namespace Cinotam.Cms.DatabaseEntities.Templates.Entities
{
    public class Template : FullAuditedEntity
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public virtual ICollection<Page> Page { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}

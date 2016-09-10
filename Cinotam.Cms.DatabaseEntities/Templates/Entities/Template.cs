using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Collections.Generic;

namespace Cinotam.Cms.DatabaseEntities.Templates.Entities
{
    public class Template : FullAuditedEntity, ITemplateContent
    {

        public virtual ICollection<Page> Page { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
    }
}

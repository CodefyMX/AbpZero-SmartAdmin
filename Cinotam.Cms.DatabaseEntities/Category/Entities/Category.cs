using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Collections.Generic;

namespace Cinotam.Cms.DatabaseEntities.Category.Entities
{
    public class Category : FullAuditedEntity, IMustHaveTenant
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<CategoryContent> CategoryContents { get; set; }
        public int TenantId { get; set; }
    }
}

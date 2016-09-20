using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSectionItem : FullAuditedEntity, IHasSection
    {
        public string Name { get; set; }
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual MenuSection MenuSection { get; set; }
        public virtual ICollection<MenuSectionItemContent> MenuSectionItemContents { get; set; }
    }
}

using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSectionContent : FullAuditedEntity, ILocalizable, IHasSection
    {
        public string Lang { get; set; }
        public string DisplayText { get; set; }

        public int SectionId { get; set; }

        [ForeignKey("SectionId")]
        public virtual MenuSection MenuSection { get; set; }
        public virtual ICollection<MenuSectionItem> MenuSectionItems { get; set; }
    }
}

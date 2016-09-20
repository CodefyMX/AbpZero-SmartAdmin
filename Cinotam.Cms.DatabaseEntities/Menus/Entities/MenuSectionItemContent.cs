using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSectionItemContent : FullAuditedEntity, IHasSectionItem, ILocalizable
    {
        public string DisplayText { get; set; }
        [ForeignKey("SectionItemId")]
        public virtual MenuSectionItem MenuSectionItem { get; set; }
        public int SectionItemId { get; set; }
        public string Lang { get; set; }
    }
}

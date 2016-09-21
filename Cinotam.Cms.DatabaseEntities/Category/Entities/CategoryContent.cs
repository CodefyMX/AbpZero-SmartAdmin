using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Category.Entities
{
    public class CategoryContent : FullAuditedEntity, IMustHaveCategory, ILocalizable
    {
        public string Lang { get; set; }
        public string DisplayText { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}

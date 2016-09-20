using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuContent : FullAuditedEntity, IHasMenu, ILocalizable
    {
        public string Text { get; set; }
        public string Lang { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
        public string Url { get; set; }

        public virtual int MenuId { get; set; }
    }
}

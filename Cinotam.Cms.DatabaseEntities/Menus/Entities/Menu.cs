using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.Collections.Generic;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class Menu : FullAuditedEntity, IHasOrder, IIsActivable
    {
        public string MenuName { get; set; }
        public virtual ICollection<MenuContent> MenuContents { get; set; }
        public virtual ICollection<MenuSection> MenuSections { get; set; }

        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}

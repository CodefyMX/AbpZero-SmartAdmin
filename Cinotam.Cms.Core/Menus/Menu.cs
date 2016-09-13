using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Collections.Generic;

namespace Cinotam.Cms.Core.Menus
{
    public class Menu : FullAuditedEntity
    {
        public string MenuName { get; set; }
        public virtual Page Page { get; set; }
        public virtual ICollection<MenuContent> MenuContents { get; set; }
        public int ParentId { get; set; }
    }
}

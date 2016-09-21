using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSection : FullAuditedEntity, IHasMenu, IHasOrder, IIsActivable
    {
        protected MenuSection()
        {

        }
        public string SectionName { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
        public string CategoryDiscriminator { get; set; }
        public static MenuSection CreateMenuSection(string sectionName, Menu menu)
        {
            if (menu.Id == 0) throw new NullReferenceException(nameof(menu));
            return new MenuSection()
            {
                SectionName = sectionName,
                Menu = menu,
                CategoryDiscriminator = sectionName
            };
        }

        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}

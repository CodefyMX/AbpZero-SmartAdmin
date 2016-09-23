using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSection : FullAuditedEntity, IHasMenu, IHasOrder, IIsActivable, IMayHaveCategory
    {
        protected MenuSection()
        {

        }
        public string SectionName { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category.Entities.Category Category { get; set; }
        public static MenuSection CreateMenuSection(string sectionName, Menu menu)
        {
            if (menu.Id == 0) throw new NullReferenceException(nameof(menu));
            return new MenuSection()
            {
                SectionName = sectionName,
                Menu = menu,
            };
        }

        public static MenuSection CreateMenuSectionWithCategory(string sectionName, Menu menu, Category.Entities.Category category)
        {
            if (menu.Id == 0) throw new NullReferenceException(nameof(menu));
            return new MenuSection()
            {
                SectionName = sectionName,
                Menu = menu,
                Category = category
            };
        }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}

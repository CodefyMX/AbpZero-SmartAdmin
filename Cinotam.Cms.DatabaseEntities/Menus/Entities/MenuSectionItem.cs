using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSectionItem : FullAuditedEntity, IHasSection, IHasOrder, IIsActivable, IMayHavePage
    {
        protected MenuSectionItem()
        {

        }
        public string Name { get; set; }
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual MenuSection MenuSection { get; set; }
        public virtual ICollection<MenuSectionItemContent> MenuSectionItemContents { get; set; }

        public static MenuSectionItem CreateMenuSectionItem(string name, MenuSection menuSection, int? pageId)
        {
            if (menuSection.Id == 0) throw new InvalidOperationException(nameof(menuSection));
            return new MenuSectionItem()
            {
                Name = name,
                MenuSection = menuSection,
                PageId = pageId
            };
        }

        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int? PageId { get; set; }
    }
}

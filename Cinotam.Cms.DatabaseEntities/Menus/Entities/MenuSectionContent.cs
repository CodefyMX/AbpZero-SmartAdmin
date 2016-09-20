using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSectionContent : FullAuditedEntity, ILocalizable, IHasSection
    {
        protected MenuSectionContent()
        {

        }
        public string Lang { get; set; }
        public string DisplayText { get; set; }

        public int SectionId { get; set; }

        [ForeignKey("SectionId")]
        public virtual MenuSection MenuSection { get; set; }
        public virtual ICollection<MenuSectionItem> MenuSectionItems { get; set; }

        public static MenuSectionContent CreateMenuSectionContent(string lang, string displayText, MenuSection menuSection)
        {
            if (menuSection.Id == 0) throw new NullReferenceException(nameof(menuSection));
            return new MenuSectionContent()
            {
                Lang = lang,
                DisplayText = displayText,
                MenuSection = menuSection
            };
        }
    }
}

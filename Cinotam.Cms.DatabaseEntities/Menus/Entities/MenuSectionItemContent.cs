using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuSectionItemContent : FullAuditedEntity, IHasSectionItem, ILocalizable, IMayHavePage
    {
        protected MenuSectionItemContent()
        {

        }
        public string DisplayText { get; set; }
        [ForeignKey("SectionItemId")]
        public virtual MenuSectionItem MenuSectionItem { get; set; }
        public int SectionItemId { get; set; }

        public int? PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }
        public string Url { get; set; }
        public string Lang { get; set; }

        public static MenuSectionItemContent CreateMenuSectionItemContent(string displayText, string lang, MenuSectionItem menuSectionItem)
        {
            if (menuSectionItem.Id == 0) throw new NullReferenceException(nameof(menuSectionItem));
            return new MenuSectionItemContent()
            {
                Lang = lang,
                MenuSectionItem = menuSectionItem,
                DisplayText = displayText
            };
        }
    }
}

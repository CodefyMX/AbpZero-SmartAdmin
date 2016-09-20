using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Menus.Entities
{
    public class MenuContent : FullAuditedEntity, IHasMenu, ILocalizable
    {
        protected MenuContent()
        {

        }
        public string Text { get; set; }
        public string Lang { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu Menu { get; set; }
        public string Url { get; set; }

        public virtual int MenuId { get; set; }

        public static MenuContent CreateMenuContent(string lang, string text, string url, Menu menu)
        {
            if (menu.Id == 0) throw new NullReferenceException(nameof(menu));
            return new MenuContent()
            {
                Lang = lang,
                Text = text,
                Url = url,
                Menu = menu
            };
        }
    }
}

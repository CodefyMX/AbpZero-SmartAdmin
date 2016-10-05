using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    public class Page : FullAuditedEntity, IMayHaveCategory, IMaybeInMenu
    {
        public string Name { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual int? ParentPage { get; set; }
        public virtual ICollection<Page> ChildPages { get; set; }
        public bool Active { get; set; }
        public string TemplateName { get; set; }
        public bool IsMainPage { get; set; }
        [ForeignKey("CategoryId")]
        public Category.Entities.Category Category { get; set; }
        public int? CategoryId { get; set; }
        public bool IncludeInMenu { get; set; }
        public bool ShowBreadCrum { get; set; }
        public bool BreadCrumInContainer { get; set; }
    }
}

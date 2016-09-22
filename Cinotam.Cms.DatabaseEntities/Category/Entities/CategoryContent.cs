using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Category.Entities
{
    public class CategoryContent : FullAuditedEntity, IMustHaveCategory, ILocalizable
    {
        protected CategoryContent()
        {

        }
        public string Lang { get; set; }
        public string DisplayText { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public static CategoryContent CreateCategoryContent(string lang, string displayText, Category category)
        {
            if (category.Id == 0) throw new NullReferenceException(nameof(category));
            return new CategoryContent()
            {
                Category = category,
                Lang = lang,
                DisplayText = displayText
            };
        }
    }
}

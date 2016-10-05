using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Templates.Entities
{
    public class Resource : FullAuditedEntity, IResource, IHasTemplate
    {
        private Template _template;
        public string ResourceType { get; set; }
        public string ResourceUrl { get; set; }
        public virtual ITemplateContent Template { get { return _template; } set { _template = value as Template; } }
        [ForeignKey("TemplateId")]
        public virtual Template TemplateObj { get { return _template; } set { _template = value; } }
        public string Description { get; set; }
        public bool IsCdn { get; set; }
        public virtual int TemplateId { get; set; }
    }
}
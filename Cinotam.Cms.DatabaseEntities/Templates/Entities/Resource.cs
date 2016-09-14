using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.DatabaseEntities.Templates.Entities
{
    public class Resource : FullAuditedEntity, IResource
    {
        private Template _template;
        public string ResourceType { get; set; }
        public string ResourceUrl { get; set; }
        public virtual ITemplateContent Template { get { return _template; } set { _template = value as Template; } }
        public virtual Template TemplateObj { get { return _template; } set { _template = value; } }

    }
}
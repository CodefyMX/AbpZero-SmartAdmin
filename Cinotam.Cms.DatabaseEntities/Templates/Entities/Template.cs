using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.DatabaseEntities.Templates.Entities
{
    public class Template : FullAuditedEntity, ITemplateContent
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
    }
}

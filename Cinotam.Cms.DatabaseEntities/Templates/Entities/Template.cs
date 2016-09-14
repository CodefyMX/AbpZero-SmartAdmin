using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Cinotam.Cms.DatabaseEntities.Templates.Entities
{
    public class Template : FullAuditedEntity, ITemplateContent
    {
        private List<Resource> _resources;

        public Template()
        {
            _resources = new List<Resource>();
        }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public bool IsPartial { get; set; }
        public virtual ICollection<IResource> Resources { get { return _resources.ConvertAll(r => (IResource)r); } }
        public virtual ICollection<Resource> ResourcesObj
        {
            get { return _resources; }
            set
            {
                _resources = value.ToList();
            }
        }

    }
}

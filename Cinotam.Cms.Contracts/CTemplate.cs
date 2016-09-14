using System.Collections.Generic;
using System.Linq;

namespace Cinotam.Cms.Contracts
{
    public class CTemplate : ITemplateContent
    {
        private List<CResource> _resources;
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public bool IsPartial { get; set; }
        public ICollection<IResource> Resources { get { return _resources.ConvertAll(r => (IResource)r); } }
        public ICollection<CResource> ResourcesObj { get { return _resources; } set { _resources = value.ToList(); } }
    }
}

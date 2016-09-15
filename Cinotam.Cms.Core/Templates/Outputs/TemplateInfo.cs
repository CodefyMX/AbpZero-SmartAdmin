using Cinotam.Cms.Contracts;
using System.Collections.Generic;

namespace Cinotam.Cms.Core.Templates.Outputs
{
    public class TemplateInfo : ITemplateContent
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public bool IsPartial { get; set; }
        public bool IsDatabaseTemplate { get; set; }
        public ICollection<IResource> Resources { get; set; }
    }
}

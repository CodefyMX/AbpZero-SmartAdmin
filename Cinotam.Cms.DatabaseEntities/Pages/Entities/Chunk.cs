using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    /// <summary>
    /// Experimental
    /// </summary>
    public class Chunk : FullAuditedEntity, IChunk
    {
        private Content _content;
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual IPageContent PageContent { get { return _content; } set { _content = value as Content; } }
        public int Order { get; set; }
        public virtual Content ContentObj { get { return _content; } set { _content = value; } }
    }
}

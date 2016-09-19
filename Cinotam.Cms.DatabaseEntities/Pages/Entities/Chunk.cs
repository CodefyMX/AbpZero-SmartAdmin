using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    /// <summary>
    /// Experimental
    /// </summary>
    public class Chunk : FullAuditedEntity, IChunk, IHasContent
    {
        private Content _content;


        public string Key { get; set; }
        public string Value { get; set; }
        public virtual IPageContent PageContent { get { return _content; } set { _content = value as Content; } }
        public int Order { get; set; }
        [ForeignKey("ContentId")]
        public virtual Content ContentObj { get { return _content; } set { _content = value; } }
        public virtual int ContentId { get; set; }
    }
}

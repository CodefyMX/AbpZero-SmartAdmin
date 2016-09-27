using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.CustomFilters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    public class Content : FullAuditedEntity, IPageContent, IHasPage, ILocalizable
    {
        public Content()
        {
            _chunks = new List<Chunk>();
        }
        private List<Chunk> _chunks;
        public string Lang { get; set; }
        public virtual int PageId { get; set; }
        public string HtmlContent { get; set; }
        public string Title { get; set; }
        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }
        /// <summary>
        /// Helper for filesystem templates
        /// </summary>
        public string TemplateUniqueName { get; set; }

        public bool IsPartial { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// May be helpfull for template changes
        /// </summary>
        public virtual ICollection<IChunk> Chunks { get { return _chunks.ConvertAll(a => (IChunk)a); } }
        public virtual ICollection<Chunk> ChunksObj { get { return _chunks; } set { _chunks = value.ToList(); } }
        public string PreviewImage { get; set; }
    }
}

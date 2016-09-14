using Abp.Domain.Entities.Auditing;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.DatabaseEntities.Pages.Entities
{
    /// <summary>
    /// Experimental
    /// </summary>
    public class Chunk : FullAuditedEntity, IChunk
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

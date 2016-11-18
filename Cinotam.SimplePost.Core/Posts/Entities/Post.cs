using Abp.Domain.Entities.Auditing;

namespace Cinotam.SimplePost.Core.Posts.Entities
{
    public class Post : FullAuditedEntity
    {
        public bool Active { get; set; }

    }
}

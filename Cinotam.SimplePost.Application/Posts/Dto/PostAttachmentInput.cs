using Abp.Application.Services.Dto;

namespace Cinotam.SimplePost.Application.Posts.Dto
{
    public class PostAttachmentInput : EntityDto
    {
        public string FileUrl { get; set; }
        public bool StoredInCdn { get; set; }
        public string Description { get; set; }
    }
}

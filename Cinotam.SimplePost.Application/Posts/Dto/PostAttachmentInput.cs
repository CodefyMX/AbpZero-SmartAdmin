using Abp.Application.Services.Dto;

namespace Cinotam.SimplePost.Application.Posts.Dto
{
    public class PostAttachmentInput : EntityDto
    {
        public string FileUrl { get; set; }
        public bool StoredInCdn { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
    }

    public class AttachmentExtraInfo
    {
        public string Info { get; set; } = "Hey!";
        public string AnotherInfo { get; set; } = "This is another info";
        public bool IsActiveSample { get; set; } = true;
    }
}

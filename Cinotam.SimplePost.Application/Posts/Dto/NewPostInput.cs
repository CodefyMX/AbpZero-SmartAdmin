using Abp.Application.Services.Dto;

namespace Cinotam.SimplePost.Application.Posts.Dto
{
    public class NewPostInput : EntityDto
    {
        public bool Active { get; set; } = true;
        public Content Content { get; set; }
    }

    public class Content : EntityDto
    {
        public string Title { get; set; }
        public string ContentString { get; set; }
        public string Lang { get; set; }

    }
}

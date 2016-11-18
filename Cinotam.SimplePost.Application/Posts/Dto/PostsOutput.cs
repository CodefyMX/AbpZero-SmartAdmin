using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Cinotam.SimplePost.Application.Posts.Dto
{
    public class PostsOutput
    {
        public IEnumerable<PostDto> PostDtos { get; set; }
    }

    public class PostDto : EntityDto
    {

        public string Title { get; set; }
        public string ContentString { get; set; }
        public string Lang { get; set; }
    }

}

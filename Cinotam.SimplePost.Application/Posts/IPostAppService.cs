using Abp.Application.Services;
using Cinotam.SimplePost.Application.Posts.Dto;
using System.Threading.Tasks;

namespace Cinotam.SimplePost.Application.Posts
{
    public interface IPostAppService : IApplicationService
    {
        Task CreateEditPost(NewPostInput input);
        Task<PostsOutput> GetPosts(string name);
        Task AddAttachment(PostAttachmentInput input);
    }
}

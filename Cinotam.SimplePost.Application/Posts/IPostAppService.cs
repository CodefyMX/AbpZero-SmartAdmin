using Abp.Application.Services;
using Cinotam.SimplePost.Application.Posts.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.SimplePost.Application.Posts
{
    public interface IPostAppService : IApplicationService
    {
        Task CreateEditPost(NewPostInput input);
        Task<PostsOutput> GetPosts(string name);
        Task AddAttachment(PostAttachmentInput input);
        Task<IEnumerable<PostAttachmentDto>> GetAttachments(int id);
        Task AddContent(Content content);
        Task<IEnumerable<Content>> GetContents(int id);
        Task<Content> GetContentForEdit(int id, string lang);
        Task DeleteContent(int id, string lang);
        Task RemoveAttachment(int id, int attachmentId);
    }
}

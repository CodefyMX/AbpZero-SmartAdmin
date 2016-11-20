using Abp.Domain.Services;
using Cinotam.SimplePost.Core.Posts.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.SimplePost.Core.Posts
{
    public interface IPostManager : IDomainService
    {
        IQueryable<Post> Posts { get; }
        Task<int> AddEditPost(Post post);
    }
}

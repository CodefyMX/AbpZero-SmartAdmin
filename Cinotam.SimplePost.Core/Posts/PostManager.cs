using Abp.Domain.Repositories;
using Cinotam.SimplePost.Core.Posts.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.SimplePost.Core.Posts
{
    public class PostManager : IPostManager
    {
        private readonly IRepository<Post> _postRepository;

        public PostManager(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public IQueryable<Post> Posts => _postRepository.GetAll();

        public async Task<int> AddEditPost(Post post)
        {
            return await _postRepository.InsertOrUpdateAndGetIdAsync(post);
        }
    }
}

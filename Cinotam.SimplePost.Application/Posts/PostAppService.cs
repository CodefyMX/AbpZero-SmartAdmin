using Cinotam.AbpModuleZero.LocalizableContent;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;
using Cinotam.SimplePost.Application.Posts.Dto;
using Cinotam.SimplePost.Core.Posts;
using Cinotam.SimplePost.Core.Posts.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.SimplePost.Application.Posts
{
    public class PostAppService : IPostAppService
    {
        private readonly ILocalizableContentManager<Post, Content> _postLocalizableContentManager;
        private readonly IPostManager _postManager;
        public PostAppService(ILocalizableContentManager<Post, Content> postLocalizableContentManager, IPostManager postManager)
        {
            _postLocalizableContentManager = postLocalizableContentManager;
            _postManager = postManager;
        }

        public async Task CreateEditPost(NewPostInput input)
        {
            var post = new Post()
            {
                Active = true
            };
            await _postManager.AddEditPost(post);
            var content = new LocalizableContent<Post, Content>(post, input.Content, input.Content.Lang);
            await _postLocalizableContentManager.CreateLocalizationContent(content);
        }

        public async Task<PostsOutput> GetPosts(string name)
        {
            var posts = _postManager.Posts.ToList();
            var contents = new List<PostDto>();
            foreach (var post in posts)
            {
                var content = await _postLocalizableContentManager.GetLocalizableContent(post, name);
                if (content == null) continue;
                var convert = LocalizableContent<Post, Content>.DeserializeContent(content.Properties);

                if (convert == null) continue;

                contents.Add(new PostDto()
                {
                    ContentString = convert.ContentString,
                    Title = convert.Title,
                    Lang = convert.Lang,
                    Id = post.Id
                });
            }
            return new PostsOutput()
            {
                PostDtos = contents
            };

        }

        public async Task<Content> GetContent(int postId)
        {
            var post = _postManager.Posts.FirstOrDefault(a => a.Id == postId);

            var contentFromPost = await _postLocalizableContentManager.GetLocalizableContent(post, CultureInfo.CurrentUICulture.Name);

            return LocalizableContent<Post, Content>.DeserializeContent(contentFromPost.Properties);

        }

    }
}

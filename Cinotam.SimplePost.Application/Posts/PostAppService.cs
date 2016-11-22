using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.UI;
using Cinotam.AbpModuleZero.Attachments;
using Cinotam.AbpModuleZero.Attachments.Contracts;
using Cinotam.AbpModuleZero.LocalizableContent;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using Cinotam.SimplePost.Application.Posts.Dto;
using Cinotam.SimplePost.Core.Posts;
using Cinotam.SimplePost.Core.Posts.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.SimplePost.Application.Posts
{
    public class PostAppService : ApplicationService, IPostAppService
    {
        private readonly ILocalizableContentManager<Post, Content> _postLocalizableContentManager;
        private readonly IPostManager _postManager;
        private readonly IAttachmentManager<Post> _attachmentManager;
        public PostAppService(ILocalizableContentManager<Post, Content> postLocalizableContentManager, IPostManager postManager, IAttachmentManager<Post> attachmentManager)
        {
            _postLocalizableContentManager = postLocalizableContentManager;
            _postManager = postManager;
            _attachmentManager = attachmentManager;
        }

        public async Task CreateEditPost(NewPostInput input)
        {
            var post = new Post()
            {
                Active = true
            };
            await _postManager.AddEditPost(post);
            input.Content.Id = post.Id;

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

        public async Task AddAttachment(PostAttachmentInput input)
        {
            var post = _postManager.Posts.FirstOrDefault(a => a.Id == input.Id);
            await _attachmentManager.AddAttachment(new HasAttachment<Post>(post, input.FileUrl, input.StoredInCdn, true, input.Description));

        }

        public async Task RemoveAttachment(int id, int attachmentId)
        {
            var entity = _postManager.Posts.FirstOrDefault(a => a.Id == id);

            await _attachmentManager.RemoveAttachment(entity, attachmentId);

        }
        public async Task<IEnumerable<PostAttachmentDto>> GetAttachments(int id)
        {
            var post = _postManager.Posts.FirstOrDefault(a => a.Id == id);
            var attachments = await _attachmentManager.GetAttachments(post);
            return attachments.Select(a => a.MapTo<PostAttachmentDto>());
        }

        public async Task AddContent(Content content)
        {
            var post = _postManager.Posts.FirstOrDefault(a => a.Id == content.Id);

            var result = await _postLocalizableContentManager.CreateLocalizationContent(new LocalizableContent<Post, Content>(post, content,
                content.Lang), true);
            if (result == LocalizationContentResult.ContentExists) throw new UserFriendlyException("ContentAlreadyExists");
            if (result == LocalizationContentResult.Error) throw new UserFriendlyException("SomeThingHappened");
        }

        public async Task<IEnumerable<Content>> GetContents(int id)
        {
            var entity = _postManager.Posts.FirstOrDefault(a => a.Id == id);

            var contents = await _postLocalizableContentManager.GetLocalizableContent(entity);

            return contents.Select(content => LocalizableContent<Post, Content>.DeserializeContent(content.Properties)).ToList();
        }

        public async Task<Content> GetContentForEdit(int id, string lang)
        {
            var entity = _postManager.Posts.FirstOrDefault(a => a.Id == id);

            var content = await _postLocalizableContentManager.GetLocalizableContent(entity, lang);

            if (content == null)
            {
                return new Content() { Lang = lang, Id = id };
            }

            var converted = LocalizableContent<Post, Content>.DeserializeContent(content.Properties);

            return new Content() { ContentString = converted.ContentString, Lang = converted.Lang, Id = id, Title = converted.Title };

        }

        public async Task<Content> GetContent(int postId)
        {
            var post = _postManager.Posts.FirstOrDefault(a => a.Id == postId);

            var contentFromPost = await _postLocalizableContentManager.GetLocalizableContent(post, CultureInfo.CurrentUICulture.Name);

            return LocalizableContent<Post, Content>.DeserializeContent(contentFromPost.Properties);

        }

        public async Task DeleteContent(int id, string lang)
        {
            var entity = _postManager.Posts.FirstOrDefault(a => a.Id == id);
            var content = await _postLocalizableContentManager.GetLocalizableContent(entity, lang);

            await _postLocalizableContentManager.DeleteContent(content);
        }
    }
}

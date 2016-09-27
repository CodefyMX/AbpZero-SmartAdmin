using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Cinotam.Cms.Contracts;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.DatabaseContentProvider.Provider
{
    public class DatabaseContentProvider : IDatabaseContentProvider
    {
        private readonly IRepository<Content> _contentRepository;
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Chunk> _chunkRepository;
        public DatabaseContentProvider(IRepository<Content> contentRepository, IRepository<Page> pageRepository, IRepository<Chunk> chunkRepository)
        {
            _contentRepository = contentRepository;
            _pageRepository = pageRepository;
            _chunkRepository = chunkRepository;
        }

        public bool IsFileSystemService => false;


        public async Task SaveContent(IPageContent input)
        {
            var page = _pageRepository.Get(input.PageId);
            var pageContent = _contentRepository.FirstOrDefault(a => a.PageId == input.PageId && a.Lang == input.Lang);
            if (pageContent != null)
            {
                pageContent.HtmlContent = input.HtmlContent.Trim();
                pageContent.Lang = input.Lang;
                pageContent.PageId = input.PageId;
                pageContent.Page = page;
                pageContent.Title = input.Title;
                pageContent.Url = input.Url;
                pageContent.TemplateUniqueName = input.TemplateUniqueName;
                pageContent.IsPartial = input.IsPartial;
                pageContent.PreviewImage = input.PreviewImage;
            }
            else
            {
                pageContent = new Content()
                {
                    HtmlContent = input.HtmlContent.Trim(),
                    Lang = input.Lang,
                    PageId = input.PageId,
                    Page = page,
                    Title = input.Title,
                    Url = input.Url,
                    TemplateUniqueName = input.TemplateUniqueName,
                    IsPartial = input.IsPartial,
                    PreviewImage = input.PreviewImage
                };
            }

            await _contentRepository.InsertOrUpdateAndGetIdAsync(pageContent);
        }

        public async Task<IPageContent> GetPageContent(int pageId)
        {
            var content =
                await _contentRepository.FirstOrDefaultAsync(a => a.PageId == pageId && a.Lang == CultureInfo.CurrentUICulture.Name);
            return content;
        }

        public async Task<IPageContent> GetPageContent(int pageId, string language)
        {

            var page = _pageRepository.FirstOrDefault(a => a.Id == pageId);
            if (page == null) throw new InvalidOperationException(nameof(page));
            var content =
                await _contentRepository.FirstOrDefaultAsync(a => a.PageId == pageId && a.Lang == language);
            if (content != null) return content;
            var newContent = new Content()
            {
                Lang = language,
                PageId = pageId,
                HtmlContent = ""
            };
            return newContent;
        }

        public async Task<IPageContent> GetPageContent(string url)
        {
            var page = await _contentRepository.FirstOrDefaultAsync(a => a.Url == url);
            return page;
        }

        public async Task AddChunks(List<CChunk> inputs)
        {
            if (!inputs.IsNullOrEmpty())
            {
                foreach (var input in inputs)
                {
                    var chunk = _chunkRepository.FirstOrDefault(a => a.Key == input.Key && input.PageContent.Id == a.ContentObj.Id);
                    if (chunk == null)
                    {
                        await _chunkRepository.InsertOrUpdateAndGetIdAsync(new Chunk()
                        {
                            Value = input.Value,
                            Key = input.Key,
                            ContentObj = input.PageContent as Content,
                            Order = input.Order
                        });
                    }
                    else
                    {
                        chunk.Value = input.Value;

                        await _chunkRepository.UpdateAsync(chunk);
                    }

                }
            }

        }

        public async Task<List<IChunk>> GetChunks(int pageId, string language)
        {
            await Task.FromResult(0);
            var chunks = new List<IChunk>();
            var firstOrDefault = _contentRepository.GetAllIncluding(a => a.ChunksObj).FirstOrDefault(a => a.PageId == pageId);
            if (firstOrDefault != null)
            {
                var contents = firstOrDefault;
                foreach (var chunk in contents.ChunksObj)
                {
                    chunks.Add(new Chunk()
                    {
                        Value = chunk.Value,
                        Key = chunk.Key,
                    });
                }
            }
            return chunks;
        }
    }
}

using Cinotam.Cms.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.FileSystemContentProvider.Provider
{
    public class FileSystemContentProvider : IFileSystemContentProvider
    {
        public bool IsFileSystemService => false;
        public Task SaveContent(IPageContent input)
        {
            throw new NotImplementedException();
        }

        public Task<IPageContent> GetPageContent(int pageId)
        {
            throw new NotImplementedException();
        }

        public Task<IPageContent> GetPageContent(int pageId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<IPageContent> GetPageContent(string url)
        {
            throw new NotImplementedException();
        }

        public Task AddChunks(List<CChunk> inputs)
        {
            throw new NotImplementedException();
        }

        public Task<List<IChunk>> GetChunks(int pageId, string language)
        {
            throw new NotImplementedException();
        }
    }
}

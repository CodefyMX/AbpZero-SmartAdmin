using Cinotam.Cms.Contracts;
using System;
using System.Threading.Tasks;

namespace Cinotam.Cms.FileSystemContentProvider.Provider
{
    public class FileSystemContentProvider : IFileSystemContentProvider
    {
        public Task SaveContent(IPageContentInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IHtmlContentOutput> GetPageContent(int pageId)
        {
            throw new NotImplementedException();
        }

        public Task<IHtmlContentOutput> GetPageContent(int pageId, string language)
        {
            throw new NotImplementedException();
        }
    }
}

using Abp.Domain.Services;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.FileSystemContentProvider.Provider
{
    public interface IFileSystemContentProvider : IPageContentProvider, IDomainService
    {

    }
}

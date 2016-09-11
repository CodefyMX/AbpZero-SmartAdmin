using Abp.Domain.Services;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.FileSystemTemplateProvider.Provider
{

    public interface IFileSystemTemplateProvider : ITemplateContentProvider, IDomainService
    {

    }
}

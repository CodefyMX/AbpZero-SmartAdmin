using Abp.Domain.Services;
using Cinotam.Cms.Contracts;

namespace Cinotam.Cms.DatabaseContentProvider.Provider
{
    public interface IDatabaseContentProvider : IPageContentProvider, IDomainService
    {
    }
}

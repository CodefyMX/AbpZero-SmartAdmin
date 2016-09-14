using Abp.Domain.Services;
using Cinotam.Cms.Contracts;
using System.Threading.Tasks;

namespace Cinotam.Cms.DatabaseTemplateProvider.Provider
{
    public interface IDatabaseTemplateProvider : ITemplateContentProvider, IDomainService
    {
        Task AddJsResource(string resourceRoute, string templateName);

    }
}

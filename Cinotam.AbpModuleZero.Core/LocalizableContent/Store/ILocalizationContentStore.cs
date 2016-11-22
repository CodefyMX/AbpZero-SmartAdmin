using Abp.Domain.Services;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.LocalizableContent.Store
{
    public interface ILocalizationContentStore : IDomainService
    {
        IQueryable<AbpCinotamLocalizableContent> LocalizableContents { get; }
        IEnumerable<AbpCinotamLocalizableContent> GetLocalizableContents(string lang);
        IEnumerable<AbpCinotamLocalizableContent> GetLocalizableContents(string lang, string entityId);
        Task<int> SaveContent(AbpCinotamLocalizableContent content, int? tenantId);
        Task RemoveContent(AbpCinotamLocalizableContent content);
    }
}

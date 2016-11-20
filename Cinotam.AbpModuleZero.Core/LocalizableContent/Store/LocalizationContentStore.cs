using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;
using Cinotam.AbpModuleZero.MultiTenancy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.LocalizableContent.Store
{
    public class LocalizationContentStore : ILocalizationContentStore
    {

        private readonly IRepository<AbpCinotamLocalizableContent> _localizableContentRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        public LocalizationContentStore(IRepository<AbpCinotamLocalizableContent> localizableContentRepository, IRepository<Tenant> tenantRepository)
        {
            _localizableContentRepository = localizableContentRepository;
            _tenantRepository = tenantRepository;
        }

        public IQueryable<AbpCinotamLocalizableContent> LocalizableContents => _localizableContentRepository.GetAll();
        public IEnumerable<AbpCinotamLocalizableContent> GetLocalizableContents(string lang)
        {
            var localizableContents = _localizableContentRepository.GetAllList(a => a.Lang == lang);

            return localizableContents.ToList();
        }

        public IEnumerable<AbpCinotamLocalizableContent> GetLocalizableContents(string lang, string entityId)
        {
            var localizableContents = _localizableContentRepository.GetAllList(a => a.Lang == lang && entityId == a.EntityId);

            return localizableContents.ToList();
        }

        public async Task<int> SaveContent(AbpCinotamLocalizableContent content, int? tenantId)
        {
            if (!tenantId.HasValue) return await _localizableContentRepository.InsertOrUpdateAndGetIdAsync(content);
            var tenant = await _tenantRepository.FirstOrDefaultAsync(a => a.Id == tenantId.Value);

            if (tenant == null) throw new EntityNotFoundException(nameof(tenantId));

            content.TenantId = tenantId;
            return await _localizableContentRepository.InsertOrUpdateAndGetIdAsync(content);
        }
    }
}

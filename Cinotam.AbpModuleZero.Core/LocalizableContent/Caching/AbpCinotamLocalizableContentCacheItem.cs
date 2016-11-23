using Abp.AutoMapper;
using Cinotam.AbpModuleZero.LocalizableContent.Entities;

namespace Cinotam.AbpModuleZero.LocalizableContent.Caching
{
    [AutoMapFrom(typeof(AbpCinotamLocalizableContent))]
    public class AbpCinotamLocalizableContentCacheItem
    {
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityDtoName { get; set; }
        public string Properties { get; set; }
        public string Lang { get; set; }
    }
}

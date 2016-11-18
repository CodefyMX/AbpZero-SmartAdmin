using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;

namespace Cinotam.AbpModuleZero.LocalizableContent.Entities
{
    public class AbpCinotamLocalizableContent : FullAuditedEntity, IMayHaveTenant
    {
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityDtoName { get; set; }
        public string Properties { get; set; }

        public string Lang { get; set; }
        protected AbpCinotamLocalizableContent()
        {

        }

        public static AbpCinotamLocalizableContent CreateLocalizableContent<T, TContentType>
            (ILocalizableContent<T, TContentType> input)
            where T : class
            where TContentType : class
        {
            return new AbpCinotamLocalizableContent()
            {
                EntityName = input.EntityName,
                Lang = input.Lang,
                EntityId = input.EntityId,
                Properties = input.Properties,
                EntityDtoName = input.EntityDtoName
            };
        }

        public int? TenantId { get; set; }
    }

}

using Abp.Domain.Entities.Auditing;
using Cinotam.AbpModuleZero.Attachments.Contracts;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;

namespace Cinotam.AbpModuleZero.Attachments.Entities
{
    public class Attachment : FullAuditedEntity
    {

        protected Attachment() { }



        public string Description { get; set; }
        public string ContentUrl { get; set; }
        public bool Active { get; set; }
        public bool StoredInCdn { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }


        public static Attachment CreateAttachment<TEntity>
            (IHasAttachment<TEntity> hasAttachmentElement)
            where TEntity : class
        {


            var entityInfo = QueryObj.CreateQueryObj(hasAttachmentElement.Entity);

            return new Attachment()
            {

                Description = hasAttachmentElement.Description,
                Active = hasAttachmentElement.Active,
                ContentUrl = hasAttachmentElement.ContentUrl,
                EntityName = entityInfo.EntityName,
                EntityId = entityInfo.EntityId,
                StoredInCdn = hasAttachmentElement.StoredInCdn,

            };
        }
    }
}

using Abp.Domain.Entities;

namespace Cinotam.AbpModuleZero.Attachments.Contracts
{
    public class HasAttachment<TEntity> : IHasAttachment<TEntity> where TEntity : class 
    {
        public string Description { get; set; }
        public string ContentUrl { get; set; }
        public bool Active { get; set; }
        public bool StoredInCdn { get; set; }
        public TEntity Entity { get; set; }

        public HasAttachment(TEntity entity,
            string fileLocation,
            bool storedInCdn,
            bool active,
            string description)
        {
            StoredInCdn = storedInCdn;
            ContentUrl = fileLocation;
            Active = active;
            Description = description;
            Entity = entity;
        }
    }
}

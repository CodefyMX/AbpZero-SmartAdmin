using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Attachments.Entities;

namespace Cinotam.AbpModuleZero.Attachments.Caching
{
    [AutoMap(typeof(Attachment))]
    public class AttachmentCacheItem : EntityDto
    {
        public string Description { get; set; }
        public string ContentUrl { get; set; }
        public bool Active { get; set; }
        public bool StoredInCdn { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string FileName { get; set; }
        public string Properties { get; set; }
    }
}

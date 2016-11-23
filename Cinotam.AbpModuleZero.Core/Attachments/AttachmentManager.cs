using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Cinotam.AbpModuleZero.Attachments.Caching;
using Cinotam.AbpModuleZero.Attachments.Contracts;
using Cinotam.AbpModuleZero.Attachments.Entities;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using Cinotam.FileManager.Contracts.FileSystemHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.Attachments
{
    public class AttachmentManager<TEntity>
        : IAttachmentManager<TEntity> where TEntity : class

    {
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IAttachmentCache _attachmentCache;
        public AttachmentManager(IRepository<Attachment> attachmentRepository, IAttachmentCache attachmentCache)
        {
            _attachmentRepository = attachmentRepository;
            _attachmentCache = attachmentCache;
        }

        public async Task<Attachment> GetAttachment(int attachmentId)
        {
            var attachment = (await _attachmentCache.GetAsync(attachmentId));
            return attachment.MapTo<Attachment>();

        }

        public async Task<IEnumerable<Attachment>> GetAttachments(TEntity entity)
        {
            var queryObj = QueryObj.CreateQueryObj(entity);
            var elements = await
                _attachmentRepository.GetAllListAsync(
                    a => a.EntityName == queryObj.EntityName && a.EntityId == queryObj.EntityId);
            return elements;
        }

        public async Task AddAttachment(IHasAttachment<TEntity> attachmentInfo)
        {
            await _attachmentRepository.InsertOrUpdateAndGetIdAsync(Attachment.CreateAttachment(attachmentInfo));
        }

        public async Task AddAttachment<TProperties>(IHasAttachment<TEntity> attachmentInfo, TProperties properties)
        {
            attachmentInfo.SerializeContent(properties);

            await _attachmentRepository.InsertOrUpdateAndGetIdAsync(Attachment.CreateAttachment(attachmentInfo));
        }

        public Task RemoveAttachment(TEntity entity, int attachmentId)
        {
            var info = QueryObj.CreateQueryObj(entity);
            var attachment = _attachmentRepository.FirstOrDefault(a => a.EntityName == info.EntityName
            && a.EntityId == info.EntityId
            && a.Id == attachmentId);

            FileSystemHelper.RemoveFile(attachment.ContentUrl);

            return _attachmentRepository.DeleteAsync(attachment);
        }
    }
}

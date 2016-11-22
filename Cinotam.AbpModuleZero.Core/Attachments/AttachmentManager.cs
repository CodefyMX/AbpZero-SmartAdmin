using Abp.Domain.Repositories;
using Cinotam.AbpModuleZero.Attachments.Contracts;
using Cinotam.AbpModuleZero.Attachments.Entities;
using Cinotam.AbpModuleZero.LocalizableContent.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.Attachments
{
    public class AttachmentManager<TEntity> : IAttachmentManager<TEntity> where TEntity : class

    {
        private readonly IRepository<Attachment> _attachmentRepository;

        public AttachmentManager(IRepository<Attachment> attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public Task<Attachment> GetAttachment(int attachmentId)
        {
            return _attachmentRepository.GetAsync(attachmentId);
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
            var queryObj = QueryObj.CreateQueryObj(attachmentInfo.Entity);

            await _attachmentRepository.InsertOrUpdateAndGetIdAsync(Attachment.CreateAttachment(attachmentInfo));

        }

        public Task RemoveAttachment(int attachmentId)
        {
            var attachment = _attachmentRepository.Get(attachmentId);
            return _attachmentRepository.DeleteAsync(attachment);

        }
    }
}

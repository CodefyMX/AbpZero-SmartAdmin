using Abp.Domain.Services;
using Cinotam.AbpModuleZero.Attachments.Contracts;
using Cinotam.AbpModuleZero.Attachments.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.AbpModuleZero.Attachments
{
    public interface IAttachmentManager<TEntity> : IDomainService where TEntity : class
    {

        Task RemoveAttachment(int attachmentId);
        Task<Attachment> GetAttachment(int attachmentId);
        Task<IEnumerable<Attachment>> GetAttachments(TEntity entity);
        Task AddAttachment(IHasAttachment<TEntity> attachmentInfo);
    }
}

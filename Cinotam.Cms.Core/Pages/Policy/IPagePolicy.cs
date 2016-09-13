using System.Threading.Tasks;
using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;

namespace Cinotam.Cms.Core.Pages.Policy
{
    public interface IPagePolicy : IDomainService
    {
        void ValidatePage(Page page);
        Task ValidateContent(Content content);
    }
}

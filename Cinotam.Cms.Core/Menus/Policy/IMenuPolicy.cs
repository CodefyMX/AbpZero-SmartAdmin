using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Menus;

namespace Cinotam.Cms.Core.Menus.Policy
{
    public interface IMenuPolicy : IDomainService
    {
        void ValidateMenu(Menu menu);
        void ValidateMenuContent(MenuContent menuContent);
    }
}

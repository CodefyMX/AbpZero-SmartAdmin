using Abp.Domain.Services;
using Cinotam.Cms.DatabaseEntities.Menus;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;

namespace Cinotam.Cms.Core.Menus.Policy
{
    public interface IMenuPolicy : IDomainService
    {
        void ValidateMenu(Menu menu);
        void ValidateMenuContent(MenuContent menuContent);
    }
}

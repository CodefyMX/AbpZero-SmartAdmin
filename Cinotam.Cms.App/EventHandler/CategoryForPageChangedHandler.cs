using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.Threading;
using Cinotam.Cms.App.Events;
using Cinotam.Cms.App.Menus;

namespace Cinotam.Cms.App.EventHandler
{
    public class CategoryForPageChangedHandler : IEventHandler<PageCategoryChangedData>, ITransientDependency
    {
        private readonly IMenuService _menuService;

        public CategoryForPageChangedHandler(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public void HandleEvent(PageCategoryChangedData eventData)
        {
            AsyncHelper.RunSync(() => _menuService.UpdateMenuItemsWhenCategoryChange(eventData.PageId, eventData.OldCategoryId, eventData.CategoryId));
        }
    }
}

using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.Threading;
using Cinotam.Cms.App.Events;
using Cinotam.Cms.Core.Menus;

namespace Cinotam.Cms.App.EventHandler
{
    public class PageStateChangedHandler : IEventHandler<PageStateChangedData>, ITransientDependency
    {
        private readonly IMenuManager _menuManager;

        public PageStateChangedHandler(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public void HandleEvent(PageStateChangedData eventData)
        {
            var page = eventData.Page;
            if (!page.Active)
            {
                _menuManager.RemoveSectionItemsForPage(page.Id);
            }
            else
            {
                if (page.IncludeInMenu)
                {
                    AsyncHelper.RunSync(() => _menuManager.SetItemForPage(page));
                }
                else
                {
                    _menuManager.RemoveSectionItemsForPage(page.Id);
                }
            }
        }
    }
}

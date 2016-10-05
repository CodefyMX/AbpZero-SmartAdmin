using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Cinotam.Cms.App.Events;
using Cinotam.Cms.Core.Menus;

namespace Cinotam.Cms.App.EventHandler
{
    public class PageDeleted : IEventHandler<PageDeletedData>, ITransientDependency
    {
        private readonly IMenuManager _menuManager;

        public PageDeleted(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public void HandleEvent(PageDeletedData eventData)
        {
            _menuManager.RemoveSectionItemsForPage(eventData.PageId);
        }
    }
}

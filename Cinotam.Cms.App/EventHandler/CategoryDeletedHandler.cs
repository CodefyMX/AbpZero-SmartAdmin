using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Cinotam.Cms.App.Events;
using Cinotam.Cms.DatabaseEntities.Menus.Entities;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;
using System.Collections.Generic;

namespace Cinotam.Cms.App.EventHandler
{
    public class CategoryDeletedHandler : IEventHandler<CategoryDeletedEventData>, ITransientDependency
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<MenuSection> _menuSectionRepository;
        private readonly IRepository<MenuSectionContent> _menuSectionContentRepository;
        private readonly IRepository<MenuSectionItem> _menuSectionItemRepository;
        private readonly IRepository<MenuSectionItemContent> _menuSectionItemContentRepository;
        public CategoryDeletedHandler(
            IRepository<Page> pageRepository,
            IRepository<MenuSection> menuSectionRepository,
            IRepository<MenuSectionContent> menuSectionContentRepository,
            IRepository<MenuSectionItem> menuSectionItemRepository,
            IRepository<MenuSectionItemContent> menuSectionItemContentRepository)
        {
            _pageRepository = pageRepository;
            _menuSectionRepository = menuSectionRepository;
            _menuSectionContentRepository = menuSectionContentRepository;
            _menuSectionItemRepository = menuSectionItemRepository;
            _menuSectionItemContentRepository = menuSectionItemContentRepository;
        }
        public void HandleEvent(CategoryDeletedEventData eventData)
        {
            var pages = _pageRepository.GetAllList(a => a.CategoryId == eventData.CategoryId);
            RemoveCategoryFromPages(pages);
            var menuSections = _menuSectionRepository.GetAllList(a => a.CategoryId == eventData.CategoryId);
            RemoveMenuSections(menuSections);
        }

        private void RemoveCategoryFromPages(IEnumerable<Page> pages)
        {
            foreach (var page in pages)
            {
                page.Category = null;
                _pageRepository.Update(page);
            }
        }

        private void RemoveMenuSections(IEnumerable<MenuSection> menuSections)
        {
            foreach (var menuSection in menuSections)
            {
                _menuSectionRepository.Delete(menuSection);

                RemoveMenuSectionsContents(menuSection.Id);
                RemoveMenuSectionItems(menuSection.Id);
            }
        }

        private void RemoveMenuSectionItems(int id)
        {
            var menuSectionItems = _menuSectionItemRepository.GetAllList(a => a.SectionId == id);
            foreach (var menuSectionItem in menuSectionItems)
            {
                _menuSectionItemRepository.Delete(menuSectionItem);
                RemoveMenuSectionItemContents(menuSectionItem.Id);
            }
        }

        private void RemoveMenuSectionItemContents(int id)
        {
            var menuSectionItemContents = _menuSectionItemContentRepository.GetAllList(a => a.SectionItemId == id);
            foreach (var menuSectionItemContent in menuSectionItemContents)
            {
                _menuSectionItemContentRepository.Delete(menuSectionItemContent);
            }
        }

        private void RemoveMenuSectionsContents(int id)
        {
            var menuSectionContents = _menuSectionContentRepository.GetAllList(a => a.SectionId == id);
            foreach (var menuSectionContent in menuSectionContents)
            {
                _menuSectionContentRepository.Delete(menuSectionContent);
            }
        }
    }
}


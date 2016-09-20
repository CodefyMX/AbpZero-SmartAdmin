using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuElement : MenuItemAbstraction
    {
        public List<MenuElementContent> PageElementContents { get; set; } = new EditableList<MenuElementContent>();
        public List<SectionElement> SectionElements { get; set; } = new EditableList<SectionElement>();

    }
}
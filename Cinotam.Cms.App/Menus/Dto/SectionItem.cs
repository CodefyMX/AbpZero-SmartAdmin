using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class SectionItem : MenuItemAbstraction
    {
        public List<MenuElementContent> PageElementContents { get; set; } = new EditableList<MenuElementContent>();
    }
}
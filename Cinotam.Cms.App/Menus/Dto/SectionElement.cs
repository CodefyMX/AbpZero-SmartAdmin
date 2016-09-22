using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class SectionElement : MenuItemContent
    {
        public List<MenuElementContent> PageElementContents { get; set; } = new EditableList<MenuElementContent>();
        public List<SectionItem> SectionItems { get; set; } = new EditableList<SectionItem>();
    }
}
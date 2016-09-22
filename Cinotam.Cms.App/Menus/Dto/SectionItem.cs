using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class SectionItem : MenuItemContent
    {
        public string Url { get; set; }
        public bool HasPage { get; set; }
        public List<MenuElementContent> PageElementContents { get; set; } = new EditableList<MenuElementContent>();
    }
}
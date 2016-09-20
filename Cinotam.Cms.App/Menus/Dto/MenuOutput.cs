using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuOutput
    {
        public List<MenuElement> MenuElements { get; set; } = new EditableList<MenuElement>();
    }
}

using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuOutput
    {
        public List<MenuDto> MenuDtos { get; set; } = new EditableList<MenuDto>();

    }

    public class MenuDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Lang { get; set; }
        public List<MenuDto> Childs { get; set; } = new EditableList<MenuDto>();

    }
}

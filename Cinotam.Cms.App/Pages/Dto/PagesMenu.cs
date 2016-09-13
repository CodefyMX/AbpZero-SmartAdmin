using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class Menu
    {
        public List<MenuDto> Menus { get; set; }
    }

    public class MenuDto
    {
        public string DisplayText { get; set; }
        public string Url { get; set; }
        public List<MenuDto> Childs { get; set; }
    }
}

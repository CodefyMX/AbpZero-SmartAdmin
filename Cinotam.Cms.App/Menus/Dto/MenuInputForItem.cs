using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuInputForItem
    {
        public string Name { get; set; }
        public List<LangInputForItem> AvailableLangs { get; set; }
    }
}

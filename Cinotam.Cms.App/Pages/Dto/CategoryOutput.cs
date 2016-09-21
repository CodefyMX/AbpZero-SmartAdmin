using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class CategoryOutput
    {
        public List<CategoryDto> Categories { get; set; } = new EditableList<CategoryDto>();
    }

    public class CategoryDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public List<Lang> Languages { get; set; }
    }
}

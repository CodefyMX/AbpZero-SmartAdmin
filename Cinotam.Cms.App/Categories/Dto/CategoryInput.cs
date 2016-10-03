using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Categories.Dto
{
    public class CategoryInput
    {
        public string Name { get; set; }
        public List<LanguageInput> LanguageInputs { get; set; } = new EditableList<LanguageInput>();
    }

    public class LanguageInput
    {
        public string Lang { get; set; }
        public string Text { get; set; }
    }
}

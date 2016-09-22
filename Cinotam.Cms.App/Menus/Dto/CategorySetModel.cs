using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class CategorySetModel
    {
        public int MenuId { get; set; }
        public List<CategorySetInputModel> AvailableCategories { get; set; } = new EditableList<CategorySetInputModel>();
    }

    public class CategorySetInputModel
    {
        public int CategoryId { get; set; }
        public string CategoryDisplayName { get; set; }
        public bool Enabled { get; set; }
        public string NameOfMenuIsIn { get; set; }
        public int IdMenuIsIn { get; set; }
        public bool Checked { get; set; }
    }
}

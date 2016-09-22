using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Categories.Dto
{
    public class CategoryForEditModel : EntityDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<CategoryLangContent> CategoryLangContents { get; set; }

    }

    public class CategoryLangContent : EntityDto
    {
        public string Lang { get; set; }
        public string DisplayText { get; set; }
        public string Icon { get; set; }
    }
}

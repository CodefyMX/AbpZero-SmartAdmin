using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuInput : EntityDto
    {
        public string Name { get; set; }
        public List<LangInput> AvailableLangs { get; set; }
        public bool IsActive { get; set; }

    }
}

using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using Cinotam.Cms.App.Pages.Dto;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuInput : EntityDto
    {
        public string Name { get; set; }
        public int PageId { get; set; }
        public int? ParentMenuId { get; set; }
        public List<PageDto> AvaiablePages { get; set; } = new EditableList<PageDto>();
        public List<MenuDto> AvailableMenus { get; set; } = new EditableList<MenuDto>();
    }
}

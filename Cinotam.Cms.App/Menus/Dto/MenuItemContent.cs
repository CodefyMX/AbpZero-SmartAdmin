using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuItemContent : EntityDto
    {
        public string DisplayName { get; set; }
        public string Lang { get; set; }
    }
}
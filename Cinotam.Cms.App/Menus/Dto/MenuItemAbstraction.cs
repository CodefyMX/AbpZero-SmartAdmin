using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Menus.Dto
{
    public abstract class MenuItemAbstraction : EntityDto
    {
        public string DisplayName { get; set; }
        public string Lang { get; set; }
    }
}
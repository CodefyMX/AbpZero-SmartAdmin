using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class LangInput : EntityDto
    {
        public string Lang { get; set; }
        public string DisplayText { get; set; }
        public string Icon { get; set; }
        public int MenuId { get; set; }
    }
}
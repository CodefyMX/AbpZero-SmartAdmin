using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class MenuInput : EntityDto
    {
        public string DisplayText { get; set; }
        public string Lang { get; set; }
        public string Url { get; set; }
        public int PageId { get; set; }
    }
}

using Abp.Application.Services.Dto;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class ChunkRequest : EntityDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }

    }
}

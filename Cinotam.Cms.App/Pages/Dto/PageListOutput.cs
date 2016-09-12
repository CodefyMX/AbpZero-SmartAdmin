using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageListOutput
    {
        public bool IsAnyPageSetAsPrincipal { get; set; }
        public List<PageDto> Pages { get; set; } = new List<PageDto>();
    }
}

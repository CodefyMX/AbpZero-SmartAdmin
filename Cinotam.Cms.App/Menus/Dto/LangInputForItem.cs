using Cinotam.Cms.App.Pages.Dto;
using System.Collections.Generic;

namespace Cinotam.Cms.App.Menus.Dto
{
    public class LangInputForItem : LangInput
    {
        public int PageId { get; set; }
        public string Url { get; set; }
        public List<PageDto> AvailablePages { get; set; }
    }
}
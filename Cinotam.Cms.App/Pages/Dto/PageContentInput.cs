using System.Collections.Generic;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class PageContentInput
    {
        public int PageId { get; set; }
        public string HtmlContent { get; set; }
        public string Lang { get; set; }
        public List<Chunk> Chunks { get; set; }
    }

    public class Chunk
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public int Order { get; set; }
    }
}
